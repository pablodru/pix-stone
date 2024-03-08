using System.Text.Json.Serialization;

namespace Pix.Models
{
    public class Keys
    {
        public Keys(KeyInfo key, UserInfo user, AccountInfo account)
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
}
