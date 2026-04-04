using MediatR;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.CreateTable;

/// <summary>
/// Команда создания новой годовой таблицы
/// </summary>
public record CreateTableCommand(
    Guid UserId,
    string NameTable
) : IRequest<YearDTO>;