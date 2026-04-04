using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Application.Years.GetYearById;
using Microsoft.AspNetCore.Http;

namespace SMT.Application.Years.GetAllYears;

public class GetAllYearsHandler : IRequestHandler<GetAllYearsQuery, List<YearDTO>>
{
    private readonly ISMTDBContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllYearsHandler(ISMTDBContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<YearDTO>> Handle(GetAllYearsQuery request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserUserId();

        var years = await _context.Years
            .AsNoTracking()
            .Include(y => y.Quarters)
                .ThenInclude(q => q.Columns)
            .Where(y => y.UserId == userId)
            .OrderByDescending(y => y.NameTable)
            .ToListAsync(cancellationToken);

        return years.Select(y => new YearDTO(
            y.Id,
            y.UserId,
            y.NameTable,
            y.Quarters
                .OrderBy(q => q.Columns.Min(c => (int)c.Month))
                .Select(q => new QuarterDTO(
                    q.Id,
                    q.Columns
                        .OrderBy(c => c.Month)
                        .Select(c => new MonthColumnDto(
                            c.Id,
                            c.Month,
                            c.Turnover,
                            c.TaxPayable,
                            c.PaidTax
                        )).ToList()
                )).ToList()
        )).ToList();
    }

    private Guid GetCurrentUserUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("Пользователь не аутентифицирован");

        if (!Guid.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Неверный формат ID пользователя");
        }

        return userId;
    }
}
