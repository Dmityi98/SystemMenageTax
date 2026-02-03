using MediatR;
using SMT.Domain.Models;
namespace SMT.Application.Years.Commands;

public class GetYearByIdCommand : IRequest<Year>
{
    public Guid Id { get; set; }
}