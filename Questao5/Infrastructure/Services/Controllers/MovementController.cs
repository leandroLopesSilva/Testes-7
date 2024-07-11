using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovement([FromBody] CreateMovementCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { Id = result });
    }
}

