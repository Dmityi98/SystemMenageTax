using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Years.CreateTable;
using SMT.Application.Years.GetAllYears;
using SMT.Application.Years.GetYearById;
using SMT.Application.Years.UpdateTable;

namespace SMT.API.Controllers;

[Authorize]
[Controller]
[Route("api/[controller]")]
public class YearsController : Controller
{
    private readonly IMediator _mediator;

    public YearsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllYears()
    {
        var query = new GetAllYearsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetYearById(Guid id)
    {
        var userId = GetCurrentUserUserId();
        var command = new GetYearByIdCommand(id, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableCommand command)
    {
        var userId = GetCurrentUserUserId();
        var newCommand = new CreateTableCommand(userId, command.NameTable);
        var result = await _mediator.Send(newCommand);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTable([FromBody] UpdateTableCommand command)
    {
        var userId = GetCurrentUserUserId();
        var updateCommand = new UpdateTableCommand(
            userId,
            command.YearId,
            command.NameTable,
            command.YearDto);
        var result = await _mediator.Send(updateCommand);
        return Ok(result);
    }

    private Guid GetCurrentUserUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("Пользователь не аутентифицирован");

        if (!Guid.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Неверный формат ID пользователя");
        }

        return userId;
    }
}