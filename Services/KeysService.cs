using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Repositories;
using Pix.Exceptions;
using Pix.Middlewares;

namespace Pix.Services;

public class KeyService
{
    private readonly KeysRepository _keyRepository;
    private readonly UserRepository _userRepository;
    private readonly AccountRepository _accountRepository;
    private readonly TokenValidationMiddleware _tokenValidationMiddleware;


    public KeyService(KeysRepository keyRepository, UserRepository userRepository, AccountRepository accountRepository, TokenValidationMiddleware tokenValidationMiddleware)
    {
        _keyRepository = keyRepository;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _tokenValidationMiddleware = tokenValidationMiddleware;
    }

    public async Task<Keys> CreateKey(CreateKeyDTO dto)
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
        if (accountsWithKeys.Count == 0)
        {
            Bank validatedBank = _tokenValidationMiddleware.GetValidatedBank();
            await _accountRepository.CreateAccount(user.Id, validatedBank.Id, dto.Account.Number, dto.Account.Agency);
        }

        int totalKeys = 0;
        foreach (var accountWithKeys in accountsWithKeys)
        {
            totalKeys += accountWithKeys.Keys.Count;
        }

        if (totalKeys >= 20)
        {
            throw new MaximumKeysException("You cannot have more than 20 Pix Keys.");
        }

        if (accountsWithKeys.Any(account => account.Keys.Count == 5 && account.Number == dto.Account.Number && account.Agency == dto.Account.Agency))
        {
            throw new MaximumKeysException("You already have 5 Pix Keys for one of your accounts.");
        }

        Keys key = await _keyRepository.CreateKey(dto.ToEntity());
        return key;
    }
}