using MediatR;
using Notification.Contracts;
using MassTransit;

namespace Notification.Application.Features.Commands;

public record SendNotificationCommand(SendNotificationEvent Request) : IRequest<bool>;

public class SendNotificationHandler(MassTransit.IPublishEndpoint publishEndpoint)
    : IRequestHandler<SendNotificationCommand, bool>
{
    public async Task<bool> Handle(SendNotificationCommand request, CancellationToken ct)
    {
        await publishEndpoint.Publish(request.Request, ct);
        return true;
    }
}