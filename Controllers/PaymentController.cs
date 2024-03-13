using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Exceptions;
using Pix.Middlewares;
using Pix.Models;
using Pix.Services;

namespace Pix.Controllers;

[ApiController]
[Route("[controller]")]

public class PaymentController(PaymentService paymentService, TokenValidationMiddleware tokenMiddleware) : ControllerBase
{
    private readonly PaymentService _paymentService = paymentService;
    private readonly TokenValidationMiddleware _tokenMiddleware = tokenMiddleware;

    [HttpPost("/payments")]
    public async Task<IActionResult> CreatePayment(CreatePaymentDTO dto)
    {
        string? authorizationHeader = this.HttpContext.Request.Headers["Authorization"];
        Bank? validatedBank = await _tokenMiddleware.ValidateToken(authorizationHeader);
        
        CreatePaymentResponse response = await _paymentService.CreatePayment(dto, validatedBank);

        return CreatedAtAction(null, null, response);
    }
}

