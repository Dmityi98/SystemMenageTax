namespace SMT.Application.Years.GetYearById;

/// <summary>
/// DTO квартала
/// </summary>
public record QuarterDTO(
    Guid Id,
    List<MonthColumnDto> Columns
);