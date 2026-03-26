using MediatR;
using SMT.Domain.Models;
namespace SMT.Application.Years.GetYaerById;

public class GetYearByIdCommand : IRequest<YearDTO>
{
    public Guid Id { get; set; }
}