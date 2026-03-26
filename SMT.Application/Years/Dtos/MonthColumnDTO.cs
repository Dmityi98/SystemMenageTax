using SMT.Domain.Models;
using SMT.Application.Common.Mappings;  
using AutoMapper;

namespace SMT.Application.Years.GetYearById;

public class MonthColumnDto : IMapWith<MonthColumn>
{
    public Guid Id { get; set; }
    public Month Month {  get; set; }
    public decimal? Turnover { get; set; }
    public decimal? TaxPayable { get; set; }
    public decimal? PaidTax { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<MonthColumn, MonthColumnDto>().ReverseMap();
    }
}