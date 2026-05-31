using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Dtos;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.Years.GetYearById;

public class GetYearByIdHandler(
    ISMTDBContext context,
    IMapper mapper) : IRequestHandler<GetYearByIdCommand, YearDTO>
{
    public async Task<YearDTO> Handle(GetYearByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Years
            .Include(y => y.Quarters)
                .ThenInclude(q => q.Columns)
            .FirstOrDefaultAsync(y => y.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundExceptions(nameof(Year), request.Id);
        }

        if (entity.UserId != request.UserId)
        {
            throw new UnauthorizedException("У вас нет прав для просмотра этой таблицы");
        }

        entity.Quarters = entity.Quarters
            .OrderBy(q => q.Columns.Min(c => (int)c.Month))
            .ToList();

        foreach (var quarter in entity.Quarters)
        {
            quarter.Columns = quarter.Columns.OrderBy(c => c.Month).ToList();
        }

        return mapper.Map<YearDTO>(entity);
    }
}