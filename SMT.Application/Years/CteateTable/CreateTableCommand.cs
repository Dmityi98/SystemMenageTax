using System.ComponentModel.DataAnnotations;
using MediatR;
using SMT.Application.Common.Mappings;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Years.CreateTable;

public class CreateTableCommand : IRequest<YearDTO>
{
    [Required] public Guid Id { get; set; }
    [Required] public string NameTable{ get; set; }
}