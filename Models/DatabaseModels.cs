using System;
using System.Collections.Generic;

namespace Pix.Models;
public class User : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string CPF { get; set; }

    public List<Account> Accounts { get; set; }
}

public class Bank : BaseEntity
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public required string Name { get; set; }
}

public class Key : BaseEntity
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public int AccountId { get; set; }
}

public class Account : BaseEntity
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public int BankId { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
    public List<Key> Keys { get; set; }
}
