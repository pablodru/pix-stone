using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Services;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class PaymentController(PaymentService paymentService, TokenService tokenService) : ControllerBase
{
    private readonly PaymentService _paymentService = paymentService;
    private readonly TokenService _tokenService = tokenService;

    [HttpPost("/payments")]
    public async Task<IActionResult> CreatePayment(CreatePaymentDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        Bank? validatedBank = await _tokenService.ValidateToken(authorizationHeader);

        CreatePaymentResponse response = await _paymentService.CreatePayment(dto, validatedBank);

        return CreatedAtAction(null, null, response);
    }

    [HttpPut("/payments/update")]
    public async Task<IActionResult> UpdatePayment(UpdatePaymentDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        TokenService.ValidateIntern(authorizationHeader);
        var response = await _paymentService.UpdatePayment(dto);

        return CreatedAtAction(null, null, response);
    }
}