using System.Collections.Generic;

namespace Pix.Models;
    public class AccountWithKeys
    {
        public int Id { get; set; }
        public required string Number { get; set; }
        public required string Agency { get; set; }
        public int BankId { get; set; }
        public int UserId { get; set; }
        public required List<Key> Keys { get; set; }
    }
