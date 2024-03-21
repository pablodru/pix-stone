using Pix.DTOs;
using Pix.Models;
using Pix.Repositories;
using Pix.Exceptions;
using Pix.Utilities;

namespace Pix.Services;

public class KeyService(KeysRepository keyRepository, UserRepository userRepository, AccountRepository accountRepository, ValidationUtils validationUtils)
{
    private readonly KeysRepository _keyRepository = keyRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly ValidationUtils _validationUtils = validationUtils;

    public async Task<KeysToCreate> CreateKey(CreateKeyDTO dto, Bank bank)
    {
        int accountId = 0;

        _validationUtils.ValidateKeyType(dto.Key.Type, dto.Key.Value);

        User? user = await _userRepository.GetUserByCPF(dto.User.Cpf);
        ValidateUserByCPF(dto, user);

        AccountWithUserAndKeys? existingAccount = await _accountRepository.GetAccountByNumberAndAgency(dto.Account.Number, dto.Account.Agency, bank.Id);
        if (existingAccount != null) accountId = existingAccount.Id;

        ValidateExistingAccount(existingAccount, user);
        await ValidateExistingKey(dto);

        List<AccountWithKeys>? accountsWithKeys = await _accountRepository.GetAccountsByUserId(user.Id);
        ValidateTotalKeys(accountsWithKeys);

        if (existingAccount == null)
        {
            Account account = await _accountRepository.CreateAccount(user.Id, bank.Id, dto.Account.Number, dto.Account.Agency);
            accountId = account.Id;
        }

        KeysToCreate key = await _keyRepository.CreateKey(dto.ToEntity(), accountId);

        return key;
    }

    public async Task<KeyWithAccountInfo> GetKeyInformation(GetKeyDTO dto, Bank bank)
    {

        _validationUtils.ValidateKeyType(dto.Type, dto.Value);

        Key? existingKey = await _keyRepository.GetKeyByValue(dto.Value, dto.Type);
        if (existingKey == null)
        {
            throw new NotFoundException("The Key was not found.");
        }

        AccountWithUserAndBank accountWithUser = await _accountRepository.GetAccountById(existingKey.AccountId);

        var response = ModelateKeyResponse(accountWithUser, existingKey);

        return response;
    }

    public void ValidateUserByCPF(CreateKeyDTO dto, User? user)
    {
        if (user == null)
        {
            throw new NotFoundException("CPF not found.");
        }
        else if (dto.Key.Type == "CPF" && dto.Key.Value != dto.User.Cpf)
        {
            throw new InvalidKeyValueException("When the key type is CPF, the key value must be the same as the user cpf.");
        }
    }

    public static void ValidateTotalKeys(List<AccountWithKeys>? accountsWithKeys)
    {
        int totalKeys = accountsWithKeys.Sum(account => account.Keys.Count);
        if (totalKeys >= 20)
        {
            throw new MaximumKeysException("You already have the maximum of 20 keys created.");
        }
    }

    public async Task ValidateExistingKey(CreateKeyDTO dto)
    {
        Key? existingKey = await _keyRepository.GetKeyByValue(dto.Key.Value, dto.Key.Type);
        if (existingKey != null)
        {
            throw new ExistingKeyException("The key must to be unique.");
        }
    }
    public static void ValidateExistingAccount(AccountWithUserAndKeys? existingAccount, User user)
    {

        if (existingAccount != null && existingAccount.UserId != user.Id)
        {
            throw new AccountBadRequestException("This account isn't from the user.");
        }

        if (existingAccount != null && existingAccount.Keys.Count == 5)
        {
            throw new MaximumKeysException("You already have 5 keys in this account.");
        }
    }

    public KeyWithAccountInfo ModelateKeyResponse(AccountWithUserAndBank accountWithUser, Key? existingKey)
    {
        var keyInfo = new KeyInfo
        {
            Value = existingKey.Value,
            Type = existingKey.Type
        };

        var userInfo = new UserMaskedInfo
        {
            Name = accountWithUser.User.Name,
            MaskedCpf = $"{accountWithUser.User.CPF.Substring(0, 3)}******{accountWithUser.User.CPF.Substring(9)}"
        };

        var accountInfo = new AccountInfoWithBank
        {
            Number = accountWithUser.Number,
            Agency = accountWithUser.Agency,
            BankName = accountWithUser.Bank.Name,
            BankId = accountWithUser.BankId.ToString()
        };

        return new KeyWithAccountInfo(keyInfo, userInfo, accountInfo);
    }
}