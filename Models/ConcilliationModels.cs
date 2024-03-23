public class Transaction
{
    public int Id { get; set; }
    public string Status { get; set; }
}

public class ConcilliationMessage
{
    public string File { get; set; }
    public string Postback { get; set; }
    public string Date { get; set; }
    public int BankId { get; set; }
}
