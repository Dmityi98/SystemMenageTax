using System.ComponentModel.DataAnnotations;

namespace SMT.Domain.Models;

public class MonthColumn
{
    private const decimal DEFAULT_PAID_TAX = 0.6m;
    [Key]
    public Guid Id { get; set; }
    public Month Month { get; set; }    
    public decimal? Turnover { get; set; }
    public decimal? Tax => (Turnover) * DEFAULT_PAID_TAX / 100;
    public decimal? TaxPayable { get; set; }
    public decimal? PaidTax { get; set; }
    public Guid QuarterId {  get; set; }  
    public Quarter Quarter { get; set; }
}