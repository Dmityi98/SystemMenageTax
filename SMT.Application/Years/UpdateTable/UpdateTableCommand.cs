using MediatR;
using SMT.Application.Dtos;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.UpdateTable;

public record UpdateTableCommand(
    Guid UserId,
    Guid YearId,
    string NameTable,
    YearDTO YearDto
) : IRequest<YearDTO>;