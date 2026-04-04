using MediatR;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.UpdateTable;

/// <summary>
/// Команда обновления годовой таблицы
/// </summary>
public record UpdateTableCommand(
    Guid UserId,
    Guid YearId,
    string NameTable,
    YearDTO YearDto
) : IRequest<bool>;