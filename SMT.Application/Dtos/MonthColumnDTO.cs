using SMT.Domain.Models;

namespace SMT.Application.Years.GetYearById;

/// <summary>
/// DTO колонки месяца
/// </summary>
public record MonthColumnDto(
    Guid Id,
    Month Month,
    decimal? Turnover,
    decimal? TaxPayable,
    decimal? PaidTax
);