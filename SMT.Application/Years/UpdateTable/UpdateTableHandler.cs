using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Dtos;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.Years.UpdateTable;

public class UpdateTableHandler(
    ISMTDBContext context,
    IMapper mapper) : IRequestHandler<UpdateTableCommand, YearDTO>
{
    public async Task<YearDTO> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        var year = await context.Years
            .Include(y => y.Quarters)
                .ThenInclude(q => q.Columns)
            .FirstOrDefaultAsync(y => y.Id == request.YearId, cancellationToken);

        if (year == null)
        {
            throw new NotFoundExceptions(nameof(Year), request.YearId);
        }

        if (year.UserId != request.UserId)
        {
            throw new UnauthorizedException("У вас нет прав для редактирования этой таблицы");
        }

        if (!string.IsNullOrEmpty(request.NameTable))
        {
            year.NameTable = request.NameTable;
        }

        foreach (var quarterDto in request.YearDto.Quarters)
        {
            var quarter = year.Quarters.FirstOrDefault(q => q.Id == quarterDto.Id);
            if (quarter == null)
                continue;

            foreach (var columnDto in quarterDto.Columns)
            {
                var column = quarter.Columns.FirstOrDefault(c => c.Id == columnDto.Id);
                if (column == null)
                    continue;

                column.Turnover = columnDto.Turnover;
                column.TaxPayable = columnDto.TaxPayable;
                column.PaidTax = columnDto.PaidTax;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        year.Quarters = year.Quarters
            .OrderBy(q => q.Columns.Min(c => (int)c.Month))
            .ToList();

        foreach (var quarter in year.Quarters)
        {
            quarter.Columns = quarter.Columns.OrderBy(c => c.Month).ToList();
        }

        return mapper.Map<YearDTO>(year);
    }
}