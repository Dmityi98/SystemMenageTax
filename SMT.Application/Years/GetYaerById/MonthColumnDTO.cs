using SMT.Domain.Models;
using SMT.Application.Common.Mappings;  
using AutoMapper;

namespace SMT.Application.Years.GetYaerById;

public class MonthColumnDTO : IMapWith<MonthColumn>
{
    public Guid Id { get; set; }
    public Month Month {  get; set; }
    public decimal? Turnover { get; set; }
    public decimal? TaxPayable { get; set; }
    public decimal? PaidTax { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<MonthColumn, MonthColumnDTO>()
            .ForMember(i => i.Id,
                opt => opt.MapFrom(i => i.Id))
            .ForMember(c => c.Month,
                opt => opt.MapFrom(i => i.Month))
            .ForMember(t => t.Turnover,
                opt => opt.MapFrom(i => i.Turnover))
            .ForMember(t => t.TaxPayable,
                opt => opt.MapFrom(i => i.TaxPayable))
            .ForMember(t => t.PaidTax,
                opt => opt.MapFrom(i => i.PaidTax));
    }
}