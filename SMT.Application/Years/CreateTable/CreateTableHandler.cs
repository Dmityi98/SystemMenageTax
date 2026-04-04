using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.Years.CreateTable;

/// <summary>
/// Обработчик команды создания годовой таблицы
/// </summary>
public class CreateTableHandler(
    ISMTDBContext context,
    IMapper mapper) : IRequestHandler<CreateTableCommand, YearDTO>
{
    public async Task<YearDTO> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        // Проверка существования пользователя
        var userExists = await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundExceptions(nameof(User), request.UserId);
        }

        // Создание годовой таблицы с кварталами и месяцами
        var year = CreateYearWithQuarters(request.UserId, request.NameTable);

        context.Years.Add(year);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<YearDTO>(year);
    }

    /// <summary>
    /// Создаёт объект Year с 4 кварталами по 3 месяца в каждом
    /// </summary>
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