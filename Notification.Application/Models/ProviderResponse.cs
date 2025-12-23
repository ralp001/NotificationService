namespace Notification.Application.Models;

public record ProviderResponse(
    bool IsSuccess,
    string? ErrorMessage,
    string? ProviderReferenceId
);