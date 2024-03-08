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
        string authorizationHeader = this.HttpContext.Request.Headers["Authorization"];

        Keys key = await _keyService.CreateKey(dto, authorizationHeader);
        return CreatedAtAction(null, null, key);
    }

}