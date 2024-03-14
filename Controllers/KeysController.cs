using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Services;
using Pix.Middlewares;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class KeysController : ControllerBase
{
    private readonly KeyService _keyService;
    private readonly TokenService _tokenService;

    public KeysController(KeyService keyService, TokenService tokenService)
    {
        _keyService = keyService;
        _tokenService = tokenService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateKey(CreateKeyDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        Bank? validatedBank = await _tokenService.ValidateToken(authorizationHeader);

        KeysToCreate key = await _keyService.CreateKey(dto, validatedBank);
        return CreatedAtAction(null, null, key);
    }

    [HttpGet("/keys/{Type}/{Value}")]
    public async Task<IActionResult> GetKeyInformation([FromRoute] string Type, [FromRoute] string Value)
    {
        var dto = new GetKeyDTO(Type, Value);
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        Bank? validatedBank = await _tokenService.ValidateToken(authorizationHeader);

        KeyWithAccountInfo key = await _keyService.GetKeyInformation(dto, validatedBank);
        return Ok(key);
    }

}