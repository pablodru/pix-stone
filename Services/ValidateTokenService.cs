using Pix.Exceptions;
using Pix.Repositories;
using Pix.Models;

namespace Pix.Services;

public class TokenService(BankRepository bankRepository)
{
    private readonly BankRepository _bankRepository = bankRepository;
    private Bank? validatedBank;

    public async Task<Bank?> ValidateToken(string authorizationHeader)
    {

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

        validatedBank = await _bankRepository.GetBankByToken(token);

        if (validatedBank == null)
        {
            throw new TokenInvalidException("Token invalid.");
        }

        return validatedBank;
    }

    public static void ValidateIntern(string authorizationHeader)
    {
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
        string validInternToken = "aceleracaoDrivenStone";
        if (token != validInternToken)
        {
            throw new TokenInvalidException("Token invalid.");
        }

        return;
    }
}
