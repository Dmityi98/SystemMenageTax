using System.ComponentModel.DataAnnotations;

namespace SMT.Domain.Models;

public class UserProfile
{
    [Key]
    public Guid Id { get; private set; }
    
    [Required]
    public Guid UserId { get; private set; }
    
    [Required]
    public User User { get; private set; }  
    
    public DateTime IpRegistrationDateTime { get; private set; }
    [MaxLength(255)]
    public string CompanyName { get; private set; } = string.Empty;
    
    
    public DateTime EndDatePayment { get; set; }
    
    // Фабричный метод для создания профиля
    public static UserProfile Create(Guid userId)
    {
        return new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            // CompanyName можно задать позже
        };
    }
    
    // Методы для изменения данных
    public void UpdateCompanyName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            CompanyName = name;
    }
}