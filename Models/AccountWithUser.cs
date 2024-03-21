using System.Collections.Generic;

namespace Pix.Models;
public class AccountWithUserAndKeys
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public int BankId { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public required List<Key> Keys { get; set; }
}

public class AccountIncludeUser
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
}

public class AccountWithUserAndBank
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public int BankId { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public required Bank Bank { get; set; }
}