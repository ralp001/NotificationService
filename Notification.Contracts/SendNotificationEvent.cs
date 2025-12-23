namespace Notification.Contracts;

// A 'record' is perfect for messages because it is immutable
public record SendNotificationEvent(
    List<string> Emails,
    List<string> PhoneNumbers,
    string Subject,
    string Message,
    string? SmsMessage = null,
    List<string>? Cc = null,
    List<string>? Bcc = null,
    string Priority = "Normal"
);