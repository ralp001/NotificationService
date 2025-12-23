using MassTransit;
using Notification.Application.Interfaces;
using Notification.Contracts;

namespace Notification.Consumer;

public class NotificationConsumer(
    ILogger<NotificationConsumer> logger,
    ISmsService smsService,
    IEmailService emailService) : IConsumer<SendNotificationEvent>
{
    public async Task Consume(ConsumeContext<SendNotificationEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing notification for: {Subject}", message.Subject);

        // 1. Handle Email Sending
        if (message.Emails is { Count: > 0 })
        {
            var emailSuccess = await emailService.SendEmailAsync(
                message.Emails,
                message.Subject,
                message.Message,
                message.Cc,
                message.Bcc);

            if (emailSuccess)
            {
                logger.LogInformation("Email sent successfully.");
            }
            else
            {
                logger.LogError("Email delivery failed.");
                throw new Exception($"Email provider error: Failed to send to {string.Join(", ", message.Emails)}");
            }
        }

        // 2. Handle SMS Sending
        if (message.PhoneNumbers is { Count: > 0 })
        {
            // If it's a single number, use the single send method
            if (message.PhoneNumbers.Count == 1)
            {
                var smsSuccess = await smsService.SendSmsAsync(message.PhoneNumbers[0], message.SmsMessage ?? message.Message);
                if (smsSuccess) logger.LogInformation("SMS sent successfully.");
            }
            // If there are multiple, use the bulk method
            else
            {
                await smsService.SendBulkSmsAsync(message.PhoneNumbers, message.SmsMessage ?? message.Message);
                logger.LogInformation("Bulk SMS request processed.");
            }
        }
    }
}