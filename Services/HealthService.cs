using Microsoft.AspNetCore.Mvc;

namespace Pix.Services;

public class HealthService
{
    public string GetHealthMessage()
    {
        return "Tudo certo!";
    }
}