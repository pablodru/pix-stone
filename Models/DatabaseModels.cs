using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pix.Models
{
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
    }

    public class Account : BaseEntity
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "The number is required.")]
        public string Number { get; set; }
        
        [Required(ErrorMessage = "The agency is required.")]
        public string Agency { get; set; }
        
        public int BankId { get; set; }
        public Bank Bank { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public List<Key> Keys { get; set; }
    }
}
