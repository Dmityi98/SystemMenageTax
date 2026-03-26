using System.ComponentModel.DataAnnotations;
using MediatR;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.UpdateTable;

public class UpdateTableCommand : IRequest<bool>
{
    [Required] public string? NameTable { get; set; }
    [Required] public YearDTO yearDto { get;  set; }
}