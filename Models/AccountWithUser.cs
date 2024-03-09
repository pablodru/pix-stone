using System.Collections.Generic;

namespace Pix.Models;
public class AccountWithUser
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public int BankId { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public required Bank Bank { get; set; }
}