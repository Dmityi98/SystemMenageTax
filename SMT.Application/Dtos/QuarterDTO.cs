namespace SMT.Application.Dtos;

/// <summary>
/// DTO квартала
/// </summary>
public record QuarterDTO(
    Guid Id,
    List<MonthColumnDto> Columns
);