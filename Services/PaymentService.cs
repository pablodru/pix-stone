using Pix.DTOs;
using Pix.Exceptions;
using Pix.Models;
using Pix.Repositories;
using Pix.Utilities;
using Pix.RabbitMQ;

namespace Pix.Services;

public class PaymentService(ValidationUtils validationUtils, UserRepository userRepository, AccountRepository accountRepository, KeysRepository keyRepository, PaymentRepository paymentRepository, PaymentProducer paymentProducer, ConcilliationProducer concilliationProducer)
{
    private readonly ValidationUtils _validationUtils = validationUtils;
    private readonly UserRepository _userRepository = userRepository;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly KeysRepository _keyRepository = keyRepository;
    private readonly PaymentRepository _paymentRepository = paymentRepository;
    private readonly PaymentProducer _paymentProducer = paymentProducer;
    private readonly ConcilliationProducer _concilliationProducer = concilliationProducer;

    private readonly int IDEMPOTENCY_SECONDS_TOLERANCE = 30;

    public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentDTO dto, Bank bank)
    {
        _validationUtils.ValidateKeyType(dto.Destiny.Key.Type, dto.Destiny.Key.Value);

        User user = await _userRepository.GetUserByCPF(dto.Origin.User.Cpf) ?? throw new NotFoundException("User's CPF not found.");
        Account? originAccount = await _accountRepository.GetAccountByIndex(dto.Origin.Account.Number, dto.Origin.Account.Agency, bank.Id);
        ValidateOriginAccount(originAccount, user);
        
        Key? destinyKey = await _keyRepository.GetKeyByValue(dto.Destiny.Key.Value, dto.Destiny.Key.Type);
        ValidateDestinyKey(destinyKey, originAccount);
        
        var indempotenceKey = new PaymentIndempotenceKey(destinyKey.Id, originAccount.Id, dto.Amount);
        await CheckIfDuplicatedByIdempotence(indempotenceKey);

        Payment payment = await _paymentRepository.CreatePayment(dto.ToEntity(), destinyKey.Id, originAccount.Id);
        var response = new CreatePaymentResponse
        {
            Id = payment.Id
        };
        try
        {
            _paymentProducer.PublishPayment(dto, payment.Id);
        } catch
        {
            await _paymentRepository.PaymentFail(payment);
            throw new RabbitMqException("Internal Server Error. Payment failed.");
        }

        return response;
    }

    public void ValidateOriginAccount(Account? originAccount, User user)
    {
        if (originAccount == null) throw new NotFoundException("The origin account was not found.");

        if (originAccount.UserId != user.Id)
        {
            throw new AccountBadRequestException("The origin account does not match with user CPF.");
        }
    }

    public void ValidateDestinyKey(Key? destinyKey, Account? originAccount)
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

}