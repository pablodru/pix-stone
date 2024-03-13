using Pix.DTOs;
using Pix.Exceptions;
using Pix.Models;
using Pix.Repositories;
using Pix.Utilities;

namespace Pix.Services;

public class PaymentService (ValidationUtils validationUtils, AccountRepository accountRepository, KeysRepository keyRepository, PaymentRepository paymentRepository)
{
    private readonly ValidationUtils _validationUtils = validationUtils;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly KeysRepository _keyRepository = keyRepository;
    private readonly PaymentRepository _paymentRepository = paymentRepository;

    public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentDTO dto, Bank? bank)
    {
        _validationUtils.ValidateKeyType(dto.Destiny.Key.Type, dto.Destiny.Key.Value);
        
        AccountIncludeUser? originAccount = await _accountRepository.GetAccountWithUserByNumberAndAgency(dto.Origin.Account.Number, dto.Origin.Account.Agency, bank.Id);
        if (originAccount ==  null) throw new NotFoundException("The origin account was not found.");
        
        if (originAccount.User.CPF != dto.Origin.User.Cpf)
        {
            throw new AccountBadRequestException("The origin account does not match with user CPF.");
        }

        Key? destinyKey = await _keyRepository.GetKeyByValue(dto.Destiny.Key.Value, dto.Destiny.Key.Type);
        if (destinyKey == null) throw new NotFoundException("The key destiny does not match with any key.");

        Payment payment = await _paymentRepository.CreatePayment(dto.ToEntity(), destinyKey.Id, originAccount.Id);
        var response = new CreatePaymentResponse
        {
            Id = payment.Id
        };

        return response;
    }
}