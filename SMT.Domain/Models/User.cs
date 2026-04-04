using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMT.Domain.Models;

public class User
{
    // Первичный ключ
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // Имя пользователя (используется для логина)
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Хэш пароля 
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    // Refresh Token для обновления сессии
    [MaxLength(500)]
    public string? RefreshToken { get; set; }

    // Время истечения срока действия Refresh Token
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Навигационное свойство: у одного пользователя много записей Year
    public List<Year>? Years { get; set; } = new List<Year>();

    // Вспомогательные свойства (не сохраняются в БД)
    [NotMapped]
    public bool IsRefreshTokenExpired => RefreshTokenExpiryTime < DateTime.UtcNow;

    [NotMapped]
    public bool HasValidRefreshToken => !string.IsNullOrEmpty(RefreshToken) && !IsRefreshTokenExpired;
}