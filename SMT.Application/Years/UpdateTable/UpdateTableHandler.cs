using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.UpdateTable;

public class UpdateTableHandler(ISMTDBContext context) : IRequestHandler<UpdateTableCommand, bool>
{
    public Task<bool> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    
    private async Task<bool> UpdateTableUser(YearDTO yearDto,string nameTable, CancellationToken cancellationToken)
    {
        
        var yearExists = await context.Years
            .AnyAsync(y => y.UserId == yearDto.UserId, cancellationToken: cancellationToken);
        
        if (!yearExists)
            return false;

        // Обновляем колонки в каждом квартале
        foreach (var quarterDto in yearDto.Quarters)
        {
            foreach (var columnDto in quarterDto.Columns)
            {
                // Обновляем напрямую в БД без загрузки
                var updated = await context.Months
                    .Where(c => c.Id == columnDto.Id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(c => c.TaxPayable, columnDto.TaxPayable)
                        .SetProperty(c => c.PaidTax, columnDto.PaidTax)
                        .SetProperty(c => c.Turnover, columnDto.Turnover), cancellationToken: cancellationToken);
            
                // Если ничего не обновилось - колонка не найдена
                if (updated == 0)
                    return false;
            }
        }
        await context.Years.ExecuteUpdateAsync(y => y
            .SetProperty(nameT => nameT.NameTable, nameTable), cancellationToken);
        return true;
        
    }
    
}