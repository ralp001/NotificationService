using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;

namespace Notification.Application.Common.Interfaces;

public interface INotificationDbContext
{
    DbSet<NotificationLog> NotificationLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}