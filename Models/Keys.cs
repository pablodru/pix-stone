using System.Text.Json.Serialization;

namespace Pix.Models
{
    public class Keys
    {
        public Keys(KeyInfo key, User user, Account account)
        {
            Key = key;
            User = user;
            Account = account;
        }

        [JsonPropertyName("key")]
        public KeyInfo Key { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("account")]
        public Account Account { get; set; }
    }

    public class KeyInfo
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class User
    {
        public string CPF { get; set; }
    }

    public class Account
    {
        public string Number { get; set; }
        public string Agency { get; set; }
    }
}
