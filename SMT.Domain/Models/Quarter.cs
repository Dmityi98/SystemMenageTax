using System.ComponentModel.DataAnnotations;

namespace SMT.Domain.Models;

public class Quarter
{
    [Key]
    public Guid Id { get; set; }

    public List<MonthColumn> Columns { get; set; } = [];
    public Guid YearID { get; set; }
    public Year? Year { get; set; }
}