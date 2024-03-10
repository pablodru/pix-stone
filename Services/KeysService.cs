using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Repositories;
using Pix.Exceptions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Pix.Middlewares;

namespace Pix.Services;

public class KeyService
{
    private readonly KeysRepository _keyRepository;
    private readonly UserRepository _userRepository;
    private readonly AccountRepository _accountRepository;


    public KeyService(KeysRepository keyRepository, UserRepository userRepository, AccountRepository accountRepository)
    {
        _keyRepository = keyRepository;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    public async Task<KeysToCreate> CreateKey(CreateKeyDTO dto, Bank bank)
    {
        ValidateKeyType(dto.Key.Type, dto.Key.Value);

        User? user = await _userRepository.GetUserByCPF(dto.User.Cpf);
        if (user == null)
        {
            throw new NotFoundException("CPF not found.");
        }
        else if (dto.Key.Type == "CPF" && dto.Key.Value != dto.User.Cpf)
        {
            throw new InvalidKeyValueException("When the key type is CPF, the key value must be the same as the user cpf.");
        }

        Account? existingAccount = await _accountRepository.GetAccountByNumberAndAgency(dto.Account.Number, dto.Account.Agency, bank.Id);

        if (existingAccount != null && existingAccount.UserId != user.Id)
        {
            throw new AccountBadRequestException("This number and agency account already exists.");
        }

        List<AccountWithKeys>? accountsWithKeys = await _accountRepository.GetAccountsByUserId(user.Id);

        int totalKeys = 0;
        int accountId = 0;

        foreach (var accountWithKeys in accountsWithKeys)
        {

            totalKeys += accountWithKeys.Keys.Count;
            if (accountWithKeys.Number == dto.Account.Number && accountWithKeys.Agency == dto.Account.Agency && accountWithKeys.BankId == bank.Id)
            {
                accountId = accountWithKeys.Id;
            }

            if (accountWithKeys.Keys.Count == 5 && accountWithKeys.Number == dto.Account.Number && accountWithKeys.Agency == dto.Account.Agency)
            {
                throw new MaximumKeysException("You already have 5 Pix Keys for one of your accounts.");
            }

            if (accountWithKeys.BankId == bank.Id && (accountWithKeys.Number != dto.Account.Number || accountWithKeys.Agency != dto.Account.Agency))
            {
                throw new AccountBadRequestException("The user can have 1 account per PSP. Use the created account.");
            }
        }

        if (accountsWithKeys.Count == 0 || accountsWithKeys == null)
        {
            Account? account = await _accountRepository.CreateAccount(user.Id, bank.Id, dto.Account.Number, dto.Account.Agency);
            accountId = account.Id;
        }

        if (accountId == 0)
        {
            Account? account = await _accountRepository.CreateAccount(user.Id, bank.Id, dto.Account.Number, dto.Account.Agency);
            accountId = account.Id;
        }

        if (totalKeys >= 20)
        {
            throw new MaximumKeysException("You cannot have more than 20 Pix Keys.");
        }

        Key? existingKey = await _keyRepository.GetKeyByValue(dto.Key.Value, dto.Key.Type);
        if (existingKey != null)
        {
            throw new ExistingKeyException("The key must to be unique, the key already exists.");
        }

        KeysToCreate key = await _keyRepository.CreateKey(dto.ToEntity(), accountId);
        return key;
    }

    public async Task<KeyWithAccountInfo> GetKeyInformation(GetKeyDTO dto, Bank bank)
    {

        ValidateKeyType(dto.Type, dto.Value);

        Key? existingKey = await _keyRepository.GetKeyByValue(dto.Value, dto.Type);
        if (existingKey == null)
        {
            throw new NotFoundException("The Key was not found.");
        }

        AccountWithUser accountWithUser = await _accountRepository.GetAccountById(existingKey.AccountId);

        if (accountWithUser == null || accountWithUser.User == null)
        {
            throw new NotFoundException("Account or User not found.");
        }

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

        // Criar o objeto de resposta com as informações criadas
        var response = new KeyWithAccountInfo(keyInfo, userInfo, accountInfo);

        return response;
    }

    public void ValidateKeyType(string Type, string Value)
    {
        if (!Regex.IsMatch(Type, "^(CPF|Email|Phone|Random)$"))
        {
            throw new TypeNotMatchException("The type must be CPF, Email, Phone or Random.");
        }
        if (Type == "CPF" && !Regex.IsMatch(Value, @"^\d{11}$"))
        {
            throw new TypeNotMatchException("The CPF value must have 11 numbers.");
        }
        if (Type == "Phone" && !Regex.IsMatch(Value, "^[0-9]{11}$"))
        {
            throw new TypeNotMatchException("The Phone value must have 11 numbers.");
        }
        if (Type == "Email" && !Regex.IsMatch(Value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
        {
            throw new TypeNotMatchException("The Email is not valid.");
        }
    }

}