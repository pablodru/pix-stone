using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Services;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class KeysController : ControllerBase
{
    private readonly KeyService _keyService;

    public KeysController(KeyService keyService)
    {
        _keyService = keyService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateKey(CreateKeyDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];

        KeysToCreate key = await _keyService.CreateKey(dto, authorizationHeader);
        return CreatedAtAction(null, null, key);
    }

    [HttpGet("/keys/{Type}/{Value}")]
    public async Task<IActionResult> GetKeyInformation([FromRoute] string Type, [FromRoute] string Value)
    {
        var dto = new GetKeyDTO(Type, Value);
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];

        KeyWithAccountInfo key = await _keyService.GetKeyInformation(dto, authorizationHeader);
        return Ok(key);
    }

}