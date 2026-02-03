using AutoMapper;
using SMT.Application.Common.Mappings;
using SMT.Domain.Models;

namespace SMT.Application.Years.Commands;

public class YearDTO :IMapWith<Year>
{
    public Guid Id { get; set; }
    public string NameTable { get; set; }
    public List<QuarterDTO> Quarters { get; set; }= new List<QuarterDTO>();
    public double TotalForQuartet { get; set; }
    public Guid UserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Year, YearDTO>()
            .ForMember(d => d.Id,
                opt => opt.MapFrom(s => s.Id))
            .ForMember(o => o.Quarters,
                opt => opt.MapFrom(s => s.Quarters))
            .ForMember(o => o.TotalForQuartet,
                opt => opt.MapFrom(s => s.TotalForQuartet))
            .ForMember(o => o.UserId,
                opt => opt.MapFrom(s => s.UserId));
        
    }
}