namespace SecurePass.Services.Contracts;

public interface ICryptographyService
{
    string Hash(string secret);

    bool VerifyHash(string secret, string secretHash);

    string ConvertToUniqueId(string secret);
}

