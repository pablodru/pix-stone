using System.ComponentModel.DataAnnotations;
using Pix.Models;

namespace Pix.DTOs;

public class CreatePaymentDTO
{

    [Required(ErrorMessage = "The origin is required.")]
    public required OriginDTO Origin { get; set; }
    [Required(ErrorMessage = "The destiny is required.")]
    public required DestinyDTO Destiny { get; set; }
    [Required(ErrorMessage = "The amount is required.")]
    public required int Amount { get; set; }
    public string? Description { get; set; }

    public CreatePayment ToEntity()
    {

        var origin = new OriginInfo
        {
            User = new UserInfo
            {
                CPF = Origin.User.Cpf
            },
            Account = new AccountInfo
            {
                Number = Origin.Account.Number,
                Agency = Origin.Account.Agency
            }
        };

        var destiny = new DestinyInfo
        {
            Key = new KeyInfo
            {
                Value = Destiny.Key.Value,
                Type = Destiny.Key.Type
            }
        };

        var payment = new CreatePayment
        {
            Amount = Amount,
            Description = Description,
            Origin = origin,
            Destiny = destiny
        };

        return payment;
    }
}

public class OriginDTO
{
    public required UserDataDTO User { get; set; }
    public required AccountDTO Account { get; set; }
}

public class DestinyDTO
{
    public required KeyInfosDTO Key { get; set; }
}