using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Configurations;
using System.Net.Http.Json;

namespace Notification.Infrastructure.Services;

public class TermiiSmsService(
    HttpClient httpClient,
    IOptions<TermiiSettings> settings) : ISmsService
{
    private readonly TermiiSettings _settings = settings.Value;

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        var payload = new
        {
            to = phoneNumber,
            from = _settings.SenderId,
            sms = message,
            type = "plain",
            channel = "dnd",
            api_key = _settings.ApiKey
        };

        var response = await httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/api/sms/send", payload);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendBulkSmsAsync(List<string> phoneNumbers, string message)
    {
        var payload = new
        {
            to = phoneNumbers,
            from = _settings.SenderId,
            sms = message,
            type = "plain",
            channel = "dnd",
            api_key = _settings.ApiKey
        };

        var response = await httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/api/sms/send/bulk", payload);

        return response.IsSuccessStatusCode;
    }
}