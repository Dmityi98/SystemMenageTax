using AutoMapper;
using SMT.Domain.Models;
using SMT.Application.Common.Mappings;
namespace SMT.Application.Years.Commands;

public class QuarterDTO : IMapWith<Quarter>
{
    public Guid Id { get; set; }
    public List<MonthColumnDTO> Columns { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Quarter, QuarterDTO>()
            .ForMember(i => i.Id,
                opt => opt.MapFrom(i => i.Id))
            .ForMember(c => c.Columns, opt => opt.MapFrom(i => i.Columns));
    }
}