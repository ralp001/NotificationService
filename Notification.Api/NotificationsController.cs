using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Contracts;
using Notification.Contracts.Common;
using Notification.Application.Features.Commands;
using Notification.Application.Features.Queries;

namespace Notification.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendNotificationEvent request)
    {
        await mediator.Send(new SendNotificationCommand(request));
        return Accepted(new ApiResponse<string>("Accepted", "Notification queued."));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var logs = await mediator.Send(new GetHistoryQuery());
        return Ok(new ApiResponse<dynamic>(logs, "History retrieved."));
    }
}