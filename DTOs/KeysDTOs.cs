using System.ComponentModel.DataAnnotations;
using Pix.Models;

namespace Pix.DTOs;
    public class CreateKeyDTO
    {
        [Required(ErrorMessage = "The key infos are required.")]
        public KeyInfosDTO Key { get; set; }

        [Required(ErrorMessage = "The user infos are required.")]
        public UserDataDTO User { get; set; }

        [Required(ErrorMessage = "The account infos are required.")]
        public AccountDTO Account { get; set; }


        public Keys ToEntity()
    {
        // Criar objetos dos tipos corretos com base nos DTOs
        var key = new KeyInfo { Value = Key.Value, Type = Key.Type };
        var user = new User { CPF = User.Cpf };
        var account = new Account { Number = Account.Number, Agency = Account.Agency };

        // Retornar uma instância de Keys com os objetos criados
        return new Keys(key, user, account);
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
        public string Number { get; set; }

        [Required(ErrorMessage = "The Agency is required.")]
        public string Agency { get; set; }
    }
