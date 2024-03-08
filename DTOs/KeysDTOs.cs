using System.ComponentModel.DataAnnotations;
using Pix.Models;

namespace Pix.DTOs;
    public class CreateKeyDTO
    {
        [Required(ErrorMessage = "The key infos are required.")]
        public required KeyInfosDTO Key { get; set; }

        [Required(ErrorMessage = "The user infos are required.")]
        public required UserDataDTO User { get; set; }

        [Required(ErrorMessage = "The account infos are required.")]
        public required AccountDTO Account { get; set; }


        public Keys ToEntity()
    {
        // Criar objetos dos tipos corretos com base nos DTOs
        var key = new KeyInfo { Value = Key.Value, Type = Key.Type };
        var user = new UserInfo { CPF = User.Cpf };
        var account = new AccountInfo { Number = Account.Number, Agency = Account.Agency };

        // Retornar uma instância de Keys com os objetos criados
        return new Keys(key, user, account);
    }
    }

    public class KeyInfosDTO
    {
        [Required(ErrorMessage = "The key value is required.")]
        public required string Value { get; set; }

        [Required(ErrorMessage = "The key type is required.")]
        [RegularExpression("^(CPF|Email|Phone|Random)$", ErrorMessage = "The key type must be CPF, Email, Phone, or Random.")]
        public required string Type { get; set; }
    }

    public class UserDataDTO
    {
        [Required(ErrorMessage = "The CPF is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The CPF must have 11 numbers.")]
        public required string Cpf { get; set; }
    }

    public class AccountDTO
    {
        [Required(ErrorMessage = "The Account number is required.")]
        public required string Number { get; set; }

        [Required(ErrorMessage = "The Agency is required.")]
        public required string Agency { get; set; }
    }
