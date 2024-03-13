using System.ComponentModel.DataAnnotations;
using Pix.Models;

namespace Pix.DTOs;
public class CreateKeyDTO
{
    public required KeyInfosDTO Key { get; set; }
    public required UserDataDTO User { get; set; }
    public required AccountDTO Account { get; set; }


    public KeysToCreate ToEntity()
    {
        var key = new KeyInfo { Value = Key.Value, Type = Key.Type };
        var user = new UserInfo { CPF = User.Cpf };
        var account = new AccountInfo { Number = Account.Number, Agency = Account.Agency };

        return new KeysToCreate(key, user, account);
    }
}

public class KeyInfosDTO
{
    [Required(ErrorMessage = "The key value is required.")]
    public string Value { get; set; }

    [Required(ErrorMessage = "The key type is required.")]
    [RegularExpression("^(CPF|Email|Phone|Random)$", ErrorMessage = "The key type must be CPF, Email, Phone, or Random.")]
    public string Type { get; set; }
}

public class UserDataDTO
{
    [Required(ErrorMessage = "The CPF is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "The CPF must have 11 numbers.")]
    public string Cpf { get; set; }
}

public class AccountDTO
{
    [Required(ErrorMessage = "The Account number is required.")]
    [RegularExpression("^[0-9]{9}$", ErrorMessage = "The Account number must contain exactly 9 numbers.")]
    public string Number { get; set; }

    [Required(ErrorMessage = "The Agency is required.")]
    [RegularExpression("^[0-9]{4}$", ErrorMessage = "The Agency must contain exactly 4 numbers.")]
    public string Agency { get; set; }
}
