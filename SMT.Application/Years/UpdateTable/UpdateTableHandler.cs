using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.Years.UpdateTable;

/// <summary>
/// Обработчик команды обновления годовой таблицы
/// </summary>
public class UpdateTableHandler(ISMTDBContext context) : IRequestHandler<UpdateTableCommand, bool>
{
    public async Task<bool> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        // Проверка существования таблицы и прав доступа
        var year = await context.Years
            .Include(y => y.Quarters)
            .ThenInclude(q => q.Columns)
            .FirstOrDefaultAsync(y => y.Id == request.YearId, cancellationToken);

        if (year == null)
        {
            throw new NotFoundExceptions(nameof(Year), request.YearId);
        }

        // Проверка: принадлежит ли таблица пользователю
        if (year.UserId != request.UserId)
        {
            throw new UnauthorizedException("У вас нет прав для редактирования этой таблицы");
        }

        // Обновление NameTable
        if (!string.IsNullOrEmpty(request.NameTable))
        {
            year.NameTable = request.NameTable;
        }

        // Обновление данных в кварталах и колонках
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
        return true;
    }
}