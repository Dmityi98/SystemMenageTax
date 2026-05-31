namespace SMT.Application.Auth.LoginUser;

/// <summary>
/// DTO ответа при входе (токены)
/// </summary>
public record LoginUserDto(
    string UserName,
    string AccessToken,
    string RefreshToken
);