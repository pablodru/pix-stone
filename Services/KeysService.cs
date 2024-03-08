using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Repositories;
using Pix.Exceptions;

namespace Pix.Services;

public class KeyService
{
    private readonly KeysRepository _keyRepository;
    private readonly UserRepository _userRepository;
    private readonly AccountRepository _accountRepository;
    private readonly BankRepository _bankRepository;


    public KeyService(KeysRepository keyRepository, UserRepository userRepository, AccountRepository accountRepository, BankRepository bankRepository)
    {
        _keyRepository = keyRepository;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
         _bankRepository = bankRepository;
    }

    public async Task<Keys> CreateKey(CreateKeyDTO dto, string authorizationHeader)
    {
        User user = await _userRepository.GetUserByCPF(dto.User.Cpf);
        if (user == null)
        {
            throw new UserNotFoundException("CPF not found.");
        } else if (dto.Key.Type == "CPF" && dto.Key.Value != dto.User.Cpf)
        {
            throw new InvalidKeyValueException("When the key type is CPF, the key value must be the same as the user cpf.");
        }

        List<AccountWithKeys> accountsWithKeys = await _accountRepository.GetAccountsByUserId(user.Id);

        int totalKeys = 0;
        int accountId = 0;
        foreach (var accountWithKeys in accountsWithKeys)
        {
            totalKeys += accountWithKeys.Keys.Count;
            if (accountWithKeys.Number == dto.Account.Number && accountWithKeys.Agency == dto.Account.Agency)
            {
                accountId = accountWithKeys.Id;
            }
            
            if(accountWithKeys.Keys.Count == 5 && accountWithKeys.Number == dto.Account.Number && accountWithKeys.Agency == dto.Account.Agency)
            {
                throw new MaximumKeysException("You already have 5 Pix Keys for one of your accounts.");
            }
        }

        if (totalKeys >= 20)
        {
            throw new MaximumKeysException("You cannot have more than 20 Pix Keys.");
        }

        if (accountsWithKeys.Count == 0)
        {
            Bank bank = await _bankRepository.GetBankByToken(authorizationHeader);
            Account account = await _accountRepository.CreateAccount(user.Id, bank.Id, dto.Account.Number, dto.Account.Agency);
            accountId = account.Id;
        }

        Key existingKey = await _keyRepository.GetKeyByValue(dto.Key.Value, dto.Key.Type);
        if (existingKey != null)
        {
            throw new ExistingKeyException("The key must to be unique, the key already exists.");
        }

        Keys key = await _keyRepository.CreateKey(dto.ToEntity(), accountId);
        return key;
    }
}