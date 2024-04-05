using System.Text;
using System.Security.Claims;
using SecurePass.Common.Models;
using SecurePass.Data.Entities;
using Microsoft.Extensions.Options;
using SecurePass.Common.Exceptions;
using SecurePass.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SecurePass.Services;

public class JwtService(IOptions<JwtConfiguration> configuration) : IJwtService
{
    private readonly JwtConfiguration _configuration = configuration.Value;

    public static string GetClaimValue(ClaimsPrincipal user, string claimType)
    {
        var claim = user.FindFirst(claimType) 
            ?? throw new ApiException("Claim not found", Common.Constants.AppConstants.ErrorCodeEnum.Unauthorized);
            
        return claim.Value;
    }

    public JwtTokenContainerModel GetJwtTokens(User user)
    {
        var claims = new List<Claim>()
        {
            new("id", user.Id.ToString()),
            new("email", user.Email),
            new("dataKey", user.DataKey),
            new("rsaPublicKey", user.RSAPrivateKey),
            new("rsaPrivateKey", user.RSAPrivateKey),
            new("randomSalt", user.RandomSalt)
        };

        var accessToken = this.SignJwt(_configuration.AccessTokenSecret, _configuration.AccessTokenExpirationTimeInMinutes, claims);
        var refreshToken = this.SignJwt(_configuration.RefreshTokenSecret, _configuration.RefreshTokenExpirationTimeInMinutes, claims);

        var tokenContainer = new JwtTokenContainerModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return tokenContainer;
    }

    public bool ValidateAccessToken(string token)
    {
        return ValidateToken(token, _configuration.AccessTokenSecret);
    }

    public string GenerateSignupToken(TempUser user)
    {
        List<Claim> claims =
        [
            new Claim("email", user.Email)
        ];

        return SignJwt(user.HashCode, _configuration.SignupTokenExpirationTimeInMinutes, claims);
    }

    public bool ValidateSignupToken(TempUser user, string token)
    {
        return ValidateToken(token, user.HashCode);
    }

    public bool ValidateRefreshToken(string token)
    {
        return ValidateToken(token, _configuration.RefreshTokenSecret);
    }

    private bool ValidateToken(string token, string tokenSecret)
    {
        TokenValidationParameters validationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)),
            ValidIssuer = _configuration.Issuer,
            ValidAudience = _configuration.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string SignJwt(string tokenSecret, int expirationTimeInMinutes, List<Claim>? claims = null)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken token = new
        (
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(expirationTimeInMinutes),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

