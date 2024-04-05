using System.Text;
using BC = BCrypt.Net.BCrypt;
using SecurePass.Services.Contracts;
using System.Security.Cryptography;

namespace SecurePass.Services;

public class CryptographyService : ICryptographyService
{
    public string Hash(string secret)
    {
        return BC.EnhancedHashPassword(secret);
    }

    public bool VerifyHash(string secret, string secretHash)
    {
        return BC.EnhancedVerify(secret, secretHash);
    }

    public string ConvertToUniqueId(string secret)
    {
        using SHA256 sha = SHA256.Create();

        var hash = sha.ComputeHash(Encoding.Default.GetBytes(secret));
        var uniqueId = Convert.ToBase64String(hash).Replace("=", "");

        return uniqueId;
    }
}

