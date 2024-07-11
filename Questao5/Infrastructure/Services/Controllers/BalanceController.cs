using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries;

namespace Questao5.Infrastructure.Services.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BalanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public BalanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetBalance(string accountId)
    {
        var query = new GetAccountBalanceQuery { AccountId = accountId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
