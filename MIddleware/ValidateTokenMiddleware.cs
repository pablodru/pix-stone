using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Pix.Exceptions;
using Pix.Repositories;
using Pix.Models;

namespace Pix.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BankRepository _bankRepository;
        private Bank _validatedBank;

        public TokenValidationMiddleware(RequestDelegate next, BankRepository bankRepository)
        {
            _next = next;
            _bankRepository = bankRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authorizationHeader = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new TokenInvalidException("Token not send.");
            }

            _validatedBank = await _bankRepository.GetBankByToken(authorizationHeader);

            if (_validatedBank == null)
            {
                throw new TokenInvalidException("Token invalid");
            }

            await _next(context);
        }

        public Bank GetValidatedBank()
        {
            return _validatedBank;
        }
    }
}
