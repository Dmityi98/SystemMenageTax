
using SMT.Application.Dtos;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.GetAllYears;
using MediatR;
public record GetAllYearsQuery : IRequest<List<YearDTO>>;
