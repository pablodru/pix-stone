using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pix.Models;
public class User : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The CPF is required.")]
    public string CPF { get; set; }

    public List<Account> Accounts { get; set; }
}

public class Bank : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The token is required.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "The Bank name is required.")]
    public string Name { get; set; }

    public List<Account> Accounts { get; set; }
}


public class Key : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The type is required.")]
    public string Type { get; set; }

    [Required(ErrorMessage = "The value is required.")]
    public string Value { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public List<Payment> Payments { get; set; }
}

public class Account : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The number is required.")]
    public string Number { get; set; }

    [Required(ErrorMessage = "The agency is required.")]
    public string Agency { get; set; }

    [Required(ErrorMessage = "The BankId is required.")]
    public int BankId { get; set; }
    public Bank Bank { get; set; }

    [Required(ErrorMessage = "The UserId is required.")]
    public int UserId { get; set; }
    public User User { get; set; }

    public List<Key> Keys { get; set; }
    public List<Payment> Payments { get; set; }
}

public class Payment : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Status is required.")]
    public string Status { get; set; }

    [Required(ErrorMessage = "The KeyId is required.")]
    public int KeyId { get; set; }
    [JsonIgnore]
    public Key Key { get; set; }

    [Required(ErrorMessage = "The BankId is required.")]
    public int AccountId { get; set; }
    public Account Account { get; set; }

    [Required(ErrorMessage = "The Amount is required.")]
    public int Amount { get; set; }

    public string? Description { get; set; }

}