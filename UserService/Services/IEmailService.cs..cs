namespace UserService.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail);
}