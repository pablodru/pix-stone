using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Pix.Exceptions;
using Pix.Repositories;
using Pix.Models;

namespace Pix.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, BankRepository bankRepository)
        {

            if (context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }
            
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new TokenInvalidException("Token not sent.");
            }

            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new TokenInvalidException("Invalid token format.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                throw new TokenInvalidException("Token not provided.");
            }

            Bank validatedBank = await bankRepository.GetBankByToken(token);

            if (validatedBank == null)
            {
                throw new TokenInvalidException("Token invalid.");
            }

            await _next(context);
        }
    }
}
