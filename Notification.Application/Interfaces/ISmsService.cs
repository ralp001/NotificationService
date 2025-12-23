namespace Notification.Application.Interfaces;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string phoneNumber, string message);

    Task<bool> SendBulkSmsAsync(List<string> phoneNumbers, string message);
}