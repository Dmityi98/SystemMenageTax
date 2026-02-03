using System.ComponentModel.DataAnnotations;

namespace SMT.Domain.Models;

public class Year
{
    [Key]
    public Guid Id { get; set; }
    public string NameTable { get; set; }
    public List<Quarter> Quarters { get; set; }= new List<Quarter>();
    public double TotalForQuartet { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}