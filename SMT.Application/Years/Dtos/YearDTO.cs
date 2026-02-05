using AutoMapper;
using SMT.Application.Common.Mappings;
using SMT.Domain.Models;

namespace SMT.Application.Years.GetYearById;

public class YearDTO :IMapWith<Year>
{
    public Guid Id { get; set; }
    
    public string NameTable { get; set; }
    public List<QuarterDTO> Quarters { get; set; } = new List<QuarterDTO>();

    public Guid UserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Year, YearDTO>().ReverseMap();
        
    }
}