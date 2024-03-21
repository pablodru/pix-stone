namespace Pix.Models;

public class KeyWithAccountAndBank
{
    public int Id { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; }
    
    public Bank Bank { get; set; }
}