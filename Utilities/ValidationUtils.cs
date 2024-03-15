using System.Text.RegularExpressions;
using Pix.Exceptions;

namespace Pix.Utilities;

public class ValidationUtils
{
    public void ValidateKeyType(string Type, string Value)
    {
        if (Type == "CPF" && !Regex.IsMatch(Value, @"^\d{11}$"))
        {
            throw new TypeNotMatchException("The CPF value must have 11 numbers.");
        }
        if (Type == "Phone" && !Regex.IsMatch(Value, "^[0-9]{11}$"))
        {
            throw new TypeNotMatchException("The Phone value must have 11 numbers.");
        }
        if (Type == "Email" && !Regex.IsMatch(Value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
        {
            throw new TypeNotMatchException("The Email is not valid.");
        }
    }
}