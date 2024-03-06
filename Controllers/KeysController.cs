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
    public IActionResult CreateKey(CreateKeyDTO dto)
    {
        Keys key = _keyService.CreateKey(dto);
        return CreatedAtAction(null, null, key);
    }
}