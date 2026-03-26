using MediatR;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Models;
namespace SMT.Application.Years.GetYaerById;

public class GetYearByIdCommand : IRequest<YearDTO>
{
    public Guid Id { get; set; }
}