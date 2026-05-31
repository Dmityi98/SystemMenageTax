using MediatR;

namespace SMT.Application.UserCommand.RefreshToken;

/// <summary>
/// Команда обновления токена доступа
/// </summary>
public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<RefreshTokenDto>;

/// <summary>
/// DTO с новой парой токенов
/// </summary>
public record RefreshTokenDto(
    string Name,
    string AccessToken,
    string RefreshToken
);
