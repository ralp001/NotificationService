namespace Notification.Infrastructure.Configurations;

public class TermiiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.ng.termii.com";
    public string SenderId { get; set; } = string.Empty;
}