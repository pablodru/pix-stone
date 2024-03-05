using Microsoft.AspNetCore.Mvc;
using Pix.Services;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class HealthController : ControllerBase
{

    private HealthService _healthService;

    public HealthController(HealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    public IActionResult GetHealth()
    {
        string message = _healthService.GetHealthMessage();
        return Ok(message);
    }
}