using MediatR;
using SMT.Domain.Models;
namespace SMT.Application.Years.Commands;

public class GetYearByIdCommand : IRequest<YearDTO>
{
    public Guid Id { get; set; }
}