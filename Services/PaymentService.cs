using Pix.DTOs;
using Pix.Exceptions;
using Pix.Models;
using Pix.Repositories;
using Pix.Utilities;
using Pix.RabbitMQ;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Pix.Services;

public class PaymentService(ValidationUtils validationUtils, AccountRepository accountRepository, KeysRepository keyRepository, PaymentRepository paymentRepository, PaymentProducer paymentProducer, ConcilliationProducer concilliationProducer)
{
    private readonly ValidationUtils _validationUtils = validationUtils;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly KeysRepository _keyRepository = keyRepository;
    private readonly PaymentRepository _paymentRepository = paymentRepository;
    private readonly PaymentProducer _paymentProducer = paymentProducer;
    private readonly ConcilliationProducer _concilliationProducer = concilliationProducer;

    private readonly int IDEMPOTENCY_SECONDS_TOLERANCE = 30;

    public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentDTO dto, Bank? bank)
    {
        _validationUtils.ValidateKeyType(dto.Destiny.Key.Type, dto.Destiny.Key.Value);

        AccountWithUserAndBank? originAccount = await _accountRepository.GetAccountWithUserAndBank(dto.Origin.Account.Number, dto.Origin.Account.Agency, bank.Id);
        ValidateOriginAccount(originAccount, dto);

        KeyWithAccountAndBank? destinyKey = await _keyRepository.GetKeyByValueWithAccount(dto.Destiny.Key.Value, dto.Destiny.Key.Type);
        ValidateDestinyKey(destinyKey, originAccount);

        var indempotenceKey = new PaymentIndempotenceKey(destinyKey.Id, originAccount.Id, dto.Amount);
        await CheckIfDuplicatedByIdempotence(indempotenceKey);

        Payment payment = await _paymentRepository.CreatePayment(dto.ToEntity(), destinyKey.Id, originAccount.Id);
        var response = new CreatePaymentResponse
        {
            Id = payment.Id
        };

        var messageResponse = new CreatePaymentResponseMessage
        {
            Id = payment.Id,
            WebHookDestiny = destinyKey.Bank.WebHook,
            WebHookOrigin = originAccount.Bank.WebHook
        };
        _paymentProducer.PublishPayment(dto, messageResponse, destinyKey.Bank.WebHook);

        return response;
    }

    public void ValidateOriginAccount(AccountWithUserAndBank? originAccount, CreatePaymentDTO dto)
    {
        if (originAccount == null) throw new NotFoundException("The origin account was not found.");

        if (originAccount.User.CPF != dto.Origin.User.Cpf)
        {
            throw new AccountBadRequestException("The origin account does not match with user CPF.");
        }
    }

    public void ValidateDestinyKey(KeyWithAccountAndBank? destinyKey, AccountWithUserAndBank originAccount)
    {
        if (destinyKey == null) throw new NotFoundException("The key destiny does not match with any key.");

        if (destinyKey.AccountId == originAccount.Id)
        {
            throw new AccountBadRequestException("The origin account can't be the same as the destiny account.");
        }
    }

    private async Task CheckIfDuplicatedByIdempotence(PaymentIndempotenceKey key)
    {
        Payment? recentPayment = await _paymentRepository.GetPaymentByIndempotenceKey(key, IDEMPOTENCY_SECONDS_TOLERANCE);

        if (recentPayment != null)
        {
            throw new RecentPaymentException("A payment with these details was made less than 30 seconds ago.");
        }
    }

    public async Task<Payment> UpdatePayment(UpdatePaymentDTO dto)
    {
        var existingPayment = await _paymentRepository.GetPaymentById(dto.Id);
        if (existingPayment == null) throw new NotFoundException("PaymentID not found.");

        var updatedPayment = await _paymentRepository.UpdatePayment(existingPayment, dto.Status);

        return updatedPayment;
    }

    public void CreateConcilliation(ConcilliationDTO dto, Bank validatedBank)
    {
        _concilliationProducer.PublishConcilliation(dto, validatedBank);
        return;
    }

}