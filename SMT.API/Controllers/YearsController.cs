using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Years.CreateTable;
using SMT.Application.Years.GetAllYears;
using SMT.Application.Years.GetYearById;
using SMT.Application.Years.UpdateTable;

namespace SMT.API.Controllers;

/// <summary>
/// Контроллер для управления годовыми таблицами
/// </summary>
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

    /// <summary>
    /// Получить все годовые таблицы текущего пользователя
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllYears()
    {
        var query = new GetAllYearsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Получить годовую таблицу по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetYearById(Guid id)
    {
        var userId = GetCurrentUserUserId();

        var command = new GetYearByIdCommand(id, userId);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Создать новую годовую таблицу
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableCommand command)
    {
        var userId = GetCurrentUserUserId();

        var newCommand = new CreateTableCommand(userId, command.NameTable);
        var result = await _mediator.Send(newCommand);

        return Ok(result);
    }

    /// <summary>
    /// Обновить годовую таблицу
    /// </summary>
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

    /// <summary>
    /// Получить ID текущего пользователя из JWT токена
    /// </summary>
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