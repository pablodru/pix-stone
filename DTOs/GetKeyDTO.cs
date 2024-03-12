using System.ComponentModel.DataAnnotations;
using Pix.Models;

namespace Pix.DTOs;
public class GetKeyDTO
{

    public GetKeyDTO (EnumDatabase.KeyTypes type, string value)
    {
        Type = type;
        Value = value;
    }

    [Required(ErrorMessage = "The key value is required.")]
    public string Value { get; set; }

    [Required(ErrorMessage = "The key type is required.")]
    [RegularExpression("^(CPF|Email|Phone|Random)$", ErrorMessage = "The key type must be CPF, Email, Phone, or Random.")]
    public EnumDatabase.KeyTypes Type { get; set; }
}