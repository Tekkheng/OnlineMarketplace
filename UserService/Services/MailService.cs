using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace UserService.Services;

public class MailService : IEmailService
{
    private readonly IConfiguration _config;

    public MailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendWelcomeEmailAsync(string toEmail)
    {
        var subject = "Welcome to Online Marketplace!";
        var body = $"<h1>Hi {toEmail},</h1><p>Thank you for registering.</p>";

        await SendEmailAsync(toEmail, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_config["MailSettings:SenderName"], _config["MailSettings:SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["MailSettings:SmtpHost"], int.Parse(_config["MailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["MailSettings:Username"], _config["MailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}