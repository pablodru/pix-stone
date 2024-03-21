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
        // _validationUtils.ValidateKeyType(dto.Destiny.Key.Type, dto.Destiny.Key.Value);

        // AccountWithUser? originAccount = await _accountRepository.GetAccountByNumberAndAgency(dto.Origin.Account.Number, dto.Origin.Account.Agency, bank.Id);
        // ValidateOriginAccount(originAccount, dto);

        // Key? destinyKey = await _keyRepository.GetKeyByValue(dto.Destiny.Key.Value, dto.Destiny.Key.Type);
        // ValidateDestinyKey(destinyKey, originAccount);

        // var indempotenceKey = new PaymentIndempotenceKey(destinyKey.Id, originAccount.Id, dto.Amount);
        // await CheckIfDuplicatedByIdempotence(indempotenceKey);

        // Payment payment = await _paymentRepository.CreatePayment(dto.ToEntity(), destinyKey.Id, originAccount.Id);
        // var response = new CreatePaymentResponse
        // {
        //     Id = payment.Id
        // };

        // AccountWithUser destinyAccount = await _accountRepository.GetAccountById(destinyKey.AccountId); //REFACTOR

        // var messageResponse = new CreatePaymentResponseMessage
        // {
        //     Id = payment.Id,
        //     WebHookDestiny = destinyAccount.Bank.WebHook,
        //     WebHookOrigin = originAccount.Bank.WebHook
        // };
        // _paymentProducer.PublishPayment(dto, messageResponse, destinyAccount.Bank.WebHook);
        var response = new CreatePaymentResponse();
        return response;
    }

    // public void ValidateOriginAccount(AccountWithUser? originAccount, CreatePaymentDTO dto)
    // {
    //     if (originAccount == null) throw new NotFoundException("The origin account was not found.");

    //     if (originAccount.User.CPF != dto.Origin.User.Cpf)
    //     {
    //         throw new AccountBadRequestException("The origin account does not match with user CPF.");
    //     }
    // }

    // public void ValidateDestinyKey(Key? destinyKey, AccountWithUser originAccount)
    // {
    //     if (destinyKey == null) throw new NotFoundException("The key destiny does not match with any key.");

    //     if (destinyKey.AccountId == originAccount.Id)
    //     {
    //         throw new AccountBadRequestException("The origin account can't be the same as the destiny account.");
    //     }
    // }

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

    public async Task CreateConcilliation(ConcilliationDTO dto)
    {
        GenerateFile(dto.File, 100);
        ReadFile(dto.File);
    }

    public static void ReadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            using StreamReader fileReader = new(filePath);
            string? line;
            while ((line = fileReader.ReadLine()) != null)
            {
                Transaction? transaction = JsonSerializer.Deserialize<Transaction>(line);
                if (transaction != null)
                {
                    Console.WriteLine($"Id:{transaction.Id}, Status:{transaction.Status}");
                }
                else
                {
                    Console.WriteLine("Erro ao deserializar a linha.");
                }
            }
        }
        else
        {
            Console.WriteLine("O arquivo não existe.");
        }
    }

    public static void GenerateFile(string filePath, int quantity)
    {
        Random random = new();
        using StreamWriter file = File.CreateText(filePath);
        for (int i = 1; i <= quantity; i++)
        {
            int value = random.Next(1, 1000);
            Transaction transaction = new() { Id = i, Status = "SUCCESS" };
            string json = JsonSerializer.Serialize(transaction);
            file.WriteLine(json);
        }
    }
}

public class Transaction
{
    public required int Id { get; set; }
    public required string Status { get; set; }
}