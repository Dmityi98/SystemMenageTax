namespace SMT.Application.UserCommand.RegisterUser;

/// <summary>
/// DTO ответа при регистрации (без пароля!)
/// </summary>
public record RegisterUserDto(string Name, Guid Id);