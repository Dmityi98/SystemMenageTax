using SMT.Application.Years.GetYearById;

namespace SMT.Application.Dtos;

/// <summary>
/// DTO годовой таблицы
/// </summary>
public record YearDTO(
    Guid Id,
    Guid UserId,
    string NameTable,
    List<QuarterDTO> Quarters
);