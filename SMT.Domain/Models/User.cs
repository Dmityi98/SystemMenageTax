namespace SMT.Domain.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Password { get; set; }
    public Guid YearID { get; set; }
    public List<Year>? Years { get; set; } = new List<Year>();
}