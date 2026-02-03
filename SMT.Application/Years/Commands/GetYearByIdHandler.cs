using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Models;
namespace SMT.Application.Years.Commands;

public class GetYearByIdHandler(ISMTDBContext context) :
    IRequestHandler<GetYearByIdCommand, Year>
{

    public async Task<Year?> Handle(GetYearByIdCommand request,
        CancellationToken cancellationToken)
    {
        Year? userYear = await context.Years.Include(y => y.Quarters).FirstOrDefaultAsync(y => y.Id == request.Id, cancellationToken);
        
        return userYear;
    }
}