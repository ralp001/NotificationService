namespace Notification.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        List<string> recipients,
        string subject,
        string body,
        List<string>? cc = null,
        List<string>? bcc = null);
}