using MediatR;
using SMT.Domain.Models;
namespace SMT.Application.Years.GetYearById;

public class GetYearByIdCommand : IRequest<YearDTO>
{
    public Guid Id { get; set; }
}