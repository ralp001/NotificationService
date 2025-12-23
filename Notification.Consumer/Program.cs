using MassTransit;
using Notification.Consumer;
using Notification.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

// Add MassTransit for the Worker
builder.Services.AddMassTransit(x =>
{
    // 1. Tell MassTransit where the Consumer is
    x.AddConsumer<NotificationConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // 2. Automatically create the queue and link it to the Consumer
        cfg.ReceiveEndpoint("notification-service-queue", e =>
        {
        
            // NEW: THE RETRY POLICY
            e.UseMessageRetry(r =>
            {
                // Try 3 times, waiting 5 seconds between each attempt
                r.Interval(3, TimeSpan.FromSeconds(5));
            });
            e.UseRateLimit(10, TimeSpan.FromSeconds(1));
            e.ConfigureConsumer<NotificationConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();