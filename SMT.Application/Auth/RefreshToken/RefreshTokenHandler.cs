using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;

namespace SMT.Application.UserCommand.RefreshToken;

/// <summary>
/// Обработчик команды обновления токена доступа
/// </summary>
public class RefreshTokenCommandHandler(
    IJwtProvider jwtProvider,
    ISMTDBContext context) : IRequestHandler<RefreshTokenCommand, RefreshTokenDto>
{
    public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по refresh токену
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedException("Неверный refresh токен");
        }

        // Проверка срока действия refresh токена
        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh токен истёк");
        }

        // Генерация новой пары токенов
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
        };

        var newAccessToken = jwtProvider.GenerateTokenAccess(claims);
        var newRefreshToken = jwtProvider.GenerateRefreshToken();

        // Обновление refresh токена в БД
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenDto(
            user.Name,
            newAccessToken,
            newRefreshToken
        );
    }
}
