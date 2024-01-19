using SecurePass.Common.Models;
using SecurePass.Data.Entities;

namespace SecurePass.Services.Contracts;

public interface IJwtService
{
    bool ValidateAccessToken(string token);

    bool ValidateRefreshToken(string token);

    string GenerateSignupToken(TempUser user);

    JwtTokenContainerModel GetJwtTokens(User user);

    bool ValidateSignupToken(TempUser user, string token);
}

