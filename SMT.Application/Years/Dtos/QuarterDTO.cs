using AutoMapper;
using SMT.Domain.Models;
using SMT.Application.Common.Mappings;
namespace SMT.Application.Years.GetYearById;

public class QuarterDTO : IMapWith<Quarter>
{
    public Guid Id { get; set; }
    public List<MonthColumnDto> Columns { get; set; } = [];
    
    public double TotalForQuartet { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Quarter, QuarterDTO>().ReverseMap();
    }
}