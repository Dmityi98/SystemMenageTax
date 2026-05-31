using MediatR;
using SMT.Application.Dtos;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.GetYearById;

public record GetYearByIdCommand(
    Guid Id,
    Guid UserId
) : IRequest<YearDTO>;