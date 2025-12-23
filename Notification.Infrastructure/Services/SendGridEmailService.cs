using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notification.Infrastructure.Services;

public class SendGridEmailService(IOptions<SendGridSettings> settings) : IEmailService
{
    private readonly SendGridSettings _settings = settings.Value;

    public async Task<bool> SendEmailAsync(
        List<string> recipients,
        string subject,
        string body,
        List<string>? cc = null,
        List<string>? bcc = null)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var tos = recipients.Select(email => new EmailAddress(email)).ToList();

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, string.Empty, body);

        if (cc is { Count: > 0 })
            msg.AddCcs(cc.Select(email => new EmailAddress(email)).ToList());

        if (bcc is { Count: > 0 })
            msg.AddBccs(bcc.Select(email => new EmailAddress(email)).ToList());

        var response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }
}