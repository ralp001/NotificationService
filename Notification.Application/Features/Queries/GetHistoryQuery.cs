using MediatR;
using Microsoft.EntityFrameworkCore;
using Notification.Application.Common.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Application.Features.Queries;

public record GetHistoryQuery : IRequest<List<NotificationLog>>;

public class GetHistoryHandler(INotificationDbContext dbContext)
    : IRequestHandler<GetHistoryQuery, List<NotificationLog>>
{
    public async Task<List<NotificationLog>> Handle(GetHistoryQuery request, CancellationToken ct)
    {
        return await dbContext.NotificationLogs
            .OrderByDescending(x => x.SentAt)
            .ToListAsync(ct);
    }
}