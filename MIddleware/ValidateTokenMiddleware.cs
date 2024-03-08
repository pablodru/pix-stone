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
            // Obter o token de autorização do cabeçalho da solicitação
            var authorizationHeader = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new TokenInvalidException("Token not sent.");
            }

            // Realizar validação do token conforme necessário
            Bank validatedBank = await bankRepository.GetBankByToken(authorizationHeader);

            if (validatedBank == null)
            {
                throw new TokenInvalidException("Token invalid.");
            }

            await _next(context);
        }
    }
}
