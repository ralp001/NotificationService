using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<TermiiSettings>(configuration.GetSection("TermiiSettings"));
        services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));


        services.AddHttpClient<ISmsService, TermiiSmsService>();
        services.AddScoped<IEmailService, SendGridEmailService>();

        return services;
    }
}