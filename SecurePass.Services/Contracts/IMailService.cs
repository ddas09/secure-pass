namespace SecurePass.Services.Contracts;

public interface IMailService
{
    Task SendAccountCreatedEmail(string userEmail);

    Task SendAccountActivationEmail(string userEmail, string signupToken);
}

