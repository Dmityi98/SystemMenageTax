namespace SMT.Application.Years.GetYearById;

/// <summary>
/// DTO годовой таблицы
/// </summary>
public record YearDTO(
    Guid Id,
    Guid UserId,
    string NameTable,
    List<QuarterDTO> Quarters
);