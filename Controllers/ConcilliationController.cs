using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Services;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class ConcilliationController(TokenService tokenService, ConcilliationService concilliationService) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;
    private readonly ConcilliationService _concilliationService = concilliationService;

    [HttpPost("/concilliation")]
    public async Task<IActionResult> CreateConcilliation(ConcilliationDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        Bank? validatedBank = await _tokenService.ValidateToken(authorizationHeader);

        _concilliationService.CreateConcilliation(dto, validatedBank);

        return CreatedAtAction(null, null, null);
    }

}