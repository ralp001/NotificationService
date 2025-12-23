using MassTransit;
using Microsoft.AspNetCore.RateLimiting;
using Notification.Api.Middleware;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure;
using Notification.Infrastructure.Persistence;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen();           // Generates the Swagger JSON

// 2. Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100; // Max 100 requests
        opt.Window = TimeSpan.FromMinutes(1); // Per minute
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });
});
builder.Services.AddScoped<INotificationDbContext>(provider =>
    provider.GetRequiredService<NotificationDbContext>());
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Notification.Application.Features.Commands.SendNotificationHandler).Assembly);
});

var app = builder.Build();

// 3. Configure HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Serves the generated JSON
    app.UseSwaggerUI(); // Serves the visual UI (Interactive page)
}

app.UseRateLimiter();

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();