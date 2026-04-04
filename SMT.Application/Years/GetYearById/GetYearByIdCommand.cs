using MediatR;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.GetYearById;

/// <summary>
/// Команда получения годовой таблицы по ID
/// </summary>
public record GetYearByIdCommand(
    Guid Id,
    Guid UserId
) : IRequest<YearDTO>;