using Pix.DTOs;
using Pix.Exceptions;
using Pix.Models;
using Pix.Repositories;
using Pix.Utilities;
using Pix.RabbitMQ;

namespace Pix.Services;

public class PaymentService(ValidationUtils validationUtils, AccountRepository accountRepository, KeysRepository keyRepository, PaymentRepository paymentRepository, PaymentProducer paymentProducer)
{
    private readonly ValidationUtils _validationUtils = validationUtils;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly KeysRepository _keyRepository = keyRepository;
    private readonly PaymentRepository _paymentRepository = paymentRepository;
    private readonly PaymentProducer _paymentProducer = paymentProducer;

    private readonly int IDEMPOTENCY_SECONDS_TOLERANCE = 30;

    public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentDTO dto, Bank? bank)
    {
        _validationUtils.ValidateKeyType(dto.Destiny.Key.Type, dto.Destiny.Key.Value);
        
        AccountWithUser? originAccount = await _accountRepository.GetAccountByNumberAndAgency(dto.Origin.Account.Number, dto.Origin.Account.Agency, bank.Id); 
        if (originAccount == null) throw new NotFoundException("The origin account was not found.");
        
        if (originAccount.User.CPF != dto.Origin.User.Cpf)
        {
            throw new AccountBadRequestException("The origin account does not match with user CPF.");
        }

        Key? destinyKey = await _keyRepository.GetKeyByValue(dto.Destiny.Key.Value, dto.Destiny.Key.Type);
        if (destinyKey == null) throw new NotFoundException("The key destiny does not match with any key.");

        if (destinyKey.AccountId == originAccount.Id)
        {
            throw new AccountBadRequestException("The origin account can't be the same as the destiny account.");
        }

        var indempotenceKey = new PaymentIndempotenceKey(destinyKey.Id, originAccount.Id, dto.Amount);
        if (await CheckIfDuplicatedByIdempotence(indempotenceKey))
        {
            throw new RecentPaymentException("A payment with these details was made less than 30 seconds ago.");
        }

        Payment payment = await _paymentRepository.CreatePayment(dto.ToEntity(), destinyKey.Id, originAccount.Id);
        var response = new CreatePaymentResponse
        {
            Id = payment.Id
        };

        AccountWithUser destinyAccount = await _accountRepository.GetAccountById(destinyKey.AccountId);

        var messageResponse = new CreatePaymentResponseMessage
        {
            Id = payment.Id,
            WebHookDestiny = destinyAccount.Bank.WebHook,
            WebHookOrigin = originAccount.Bank.WebHook
        };
        _paymentProducer.PublishPayment(dto, messageResponse, destinyAccount.Bank.WebHook);

        return response;
    }

    private async Task<bool> CheckIfDuplicatedByIdempotence(PaymentIndempotenceKey key)
    {
        Payment? recentPayment = await _paymentRepository.GetPaymentByIndempotenceKey(key, IDEMPOTENCY_SECONDS_TOLERANCE);

        return recentPayment != null;
    }
}