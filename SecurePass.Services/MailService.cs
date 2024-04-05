using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using SecurePass.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using SecurePass.Services.Contracts;

namespace SecurePass.Services;

public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly MailConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public MailService(IWebHostEnvironment hostingEnvironment, IOptions<MailConfiguration> configuration, ILogger<MailService> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task SendAccountCreatedEmail(string userEmail)
    {        
        string mailBody = await GetMailTemplateFileContent("AccountCreatedMail");
        mailBody = mailBody.Replace("[email]", userEmail);

        var mailRequest = new MailRequest
        {
            Subject = "Account Created!",
            Body = mailBody,
            RecipientEmail = userEmail
        };

        await this.SendMail(mailRequest);
    }

    public async Task SendAccountActivationEmail(string userEmail, string signupToken)
    {
        this._logger.LogInformation("Sending account activation mail.");

        string mailBody = await GetMailTemplateFileContent("AccountActivationMail");
             
        var activationLink = $"http://localhost:4200/create-account?token={signupToken}";
        mailBody = mailBody.Replace("[email]", userEmail).Replace("[link]", activationLink);

        var mailRequest = new MailRequest
        {
            Subject = "Welcome to SecurePass!",
            Body = mailBody,
            RecipientEmail = userEmail
        };

        await this.SendMail(mailRequest);

        this._logger.LogInformation("Account activation mail sent to user.");
    }

    private async Task<string> GetMailTemplateFileContent(string fileName)
    {
        this._logger.LogInformation("Getting mail template file content.");

        var rootPath = _hostingEnvironment.ContentRootPath;
        var mailTemplateFilePath = Path.Combine(rootPath, "EmailTemplates", $"{fileName}.html");
        this._logger.LogInformation($"Mail template file path: {mailTemplateFilePath}");

        if (!File.Exists(mailTemplateFilePath))
        {
            var message = "Mail template file not found.";
            this._logger.LogError(message);
            throw new FileNotFoundException(message);
        }

        using var reader = new StreamReader(mailTemplateFilePath);
        var content = await reader.ReadToEndAsync();

        return content;
    }

    private async Task SendMail(MailRequest request)
    {
        var builder = new BodyBuilder
        {
            HtmlBody = request.Body
        };
        
        var email = new MimeMessage
        {
            Subject = request.Subject,
            Body = builder.ToMessageBody(),
            Sender = MailboxAddress.Parse(_configuration.Mail),
        };
        email.To.Add(MailboxAddress.Parse(request.RecipientEmail));
                
        using var smtp = new SmtpClient();

        smtp.Connect(_configuration.Host, _configuration.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_configuration.Username, _configuration.Password);

        await smtp.SendAsync(email);
    }
}

