namespace Pix.Models;
public class EnumDatabase
{
    public enum KeyTypes
    {
        Phone, CPF, Random, Email
    }

    public enum PaymentStatus
    {
        PROCESSING, FAILED, SUCCESS
    }
}