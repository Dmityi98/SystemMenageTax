using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;
namespace SMT.Application.Years.GetYearById;

public class GetYearByIdHandler(ISMTDBContext context, IMapper mapper) :
    IRequestHandler<GetYearByIdCommand, YearDTO>
{

    public async Task<YearDTO> Handle(GetYearByIdCommand request,
        CancellationToken cancellationToken)
    {
        Year? entity = await context.Years.
            Include(y => y.Quarters).
            ThenInclude(q=> q.Columns).
            FirstOrDefaultAsync(y => y.UserId == request.Id,
                cancellationToken: cancellationToken);
        
        if (entity == null)
        {
            throw new NotFoundExceptions(nameof(Year), request.Id);
        }
        return mapper.Map<YearDTO>(entity);
    }       
}