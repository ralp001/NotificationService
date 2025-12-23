namespace Notification.Domain.Entities;

public class NotificationLog
{
    public Guid Id { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty; // "SMS" or "Email"
    public string Provider { get; set; } = string.Empty;         // "Termii" or "SendGrid"
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}