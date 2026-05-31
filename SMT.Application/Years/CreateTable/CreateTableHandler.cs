using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Dtos;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.Years.CreateTable;

public class CreateTableHandler(
    ISMTDBContext context,
    IMapper mapper) : IRequestHandler<CreateTableCommand, YearDTO>
{
    public async Task<YearDTO> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var year = CreateYearWithQuarters(request.UserId, request.NameTable);

        context.Years.Add(year);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<YearDTO>(year);
    }

    private static Year CreateYearWithQuarters(Guid userId, string nameTable)
    {
        var year = new Year
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NameTable = nameTable,
            Quarters = new List<Quarter>()
        };

        var monthIndex = 0;

        for (var quarterIndex = 0; quarterIndex < 4; quarterIndex++)
        {
            var quarter = new Quarter
            {
                Id = Guid.NewGuid(),
                Year = year,
                YearID = year.Id,
                Columns = new List<MonthColumn>()
            };

            for (var i = 0; i < 3; i++)
            {
                var monthColumn = new MonthColumn
                {
                    Id = Guid.NewGuid(),
                    Month = (Domain.Models.Month)monthIndex++,
                    Quarter = quarter,
                    QuarterId = quarter.Id
                };
                quarter.Columns.Add(monthColumn);
            }

            year.Quarters.Add(quarter);
        }

        return year;
    }
}