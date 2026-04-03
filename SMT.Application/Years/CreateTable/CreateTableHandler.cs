using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Models;

namespace SMT.Application.Years.CreateTable;

public class CreateTableHandler(ISMTDBContext context, IMapper mapper ) : IRequestHandler<CreateTableCommand, YearDTO>
{

    public async Task<YearDTO> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.Id);

        var year = await Initialize(request.Id, request.NameTable,  cancellationToken);

        year.UserId = user.Id;
        

        return mapper.Map<YearDTO>(year);
    }

    private async Task<Year> Initialize(Guid userId,string nameTable, CancellationToken cancellationToken)
    {
        var year = new Year()
        {
            UserId = userId,
            NameTable = nameTable,
            Quarters = new List<Quarter>()
        };

        int countMonth = 0;

        for (int quarterIndex = 0; quarterIndex < 4; quarterIndex++)
        {
            var quarter = new Quarter()
            {
                Year = year,
                YearID = year.Id,
                Columns = new List<MonthColumn>()
            };
            for (int monthIndex = 0; monthIndex < 3; monthIndex++)
            {
                var month = new MonthColumn()
                {
                    Month = (Month)countMonth++,
                    Quarter = quarter,
                    QuarterId = quarter.Id
                };
                quarter.Columns.Add(month);
            }
            year.Quarters.Add(quarter);
        }
        context.Years.Add(year);
        await context.SaveChangesAsync(cancellationToken);
        return year;
    }
    
}