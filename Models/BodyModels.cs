using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace Pix.Models;

public class CreatePayment
{
    public required OriginInfo Origin { get; set; }
    public required DestinyInfo Destiny { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
}

public class OriginInfo
{
    public required UserInfo User;
    public required AccountInfo Account;
}

public class DestinyInfo
{
    public required KeyInfo Key;
}

public class KeysToCreate
{
    public KeysToCreate(KeyInfo key, UserInfo user, AccountInfo account)
    {
        Key = key;
        User = user;
        Account = account;
    }

    [JsonPropertyName("key")]
    public KeyInfo Key { get; set; }

    [JsonPropertyName("user")]
    public UserInfo User { get; set; }

    [JsonPropertyName("account")]
    public AccountInfo Account { get; set; }
}

public class KeyWithAccountInfo
{
    public KeyWithAccountInfo(KeyInfo key, UserMaskedInfo user, AccountInfoWithBank account)
    {
        Key = key;
        User = user;
        Account = account;
    }

    [JsonPropertyName("key")]
    public KeyInfo Key { get; set; }

    [JsonPropertyName("user")]
    public UserMaskedInfo User { get; set; }

    [JsonPropertyName("account")]
    public AccountInfoWithBank Account { get; set; }
}

public class KeyInfo
{
    public string Value { get; set; }
    public string Type { get; set; }
}

public class UserInfo
{
    public string CPF { get; set; }
}

public class AccountInfo
{
    public string Number { get; set; }
    public string Agency { get; set; }
}

public class UserMaskedInfo
{
    public string Name { get; set; }
    public string MaskedCpf { get; set; }
}

public class AccountInfoWithBank
{
    public string Number { get; set; }
    public string Agency { get; set; }
    public string BankName { get; set; }
    public string BankId { get; set; }
}