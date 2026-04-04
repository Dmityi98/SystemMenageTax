using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.UserCommand.LoginUser;

/// <summary>
/// Обработчик команды входа пользователя
/// </summary>
public class LoginUserCommandHandler(
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    ISMTDBContext context) : IRequestHandler<LoginUserCommand, LoginUserDto>
{
    public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

        // Безопасная проверка (не раскрываем, что именно не так)
        if (user == null || !passwordHasher.Verify(request.Password, user.Password))
        {
            throw new UnauthorizedException("Неверный логин или пароль");
        }

        // Генерация токенов
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
        };

        var accessToken = jwtProvider.GenerateTokenAccess(claims);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        // Сохранение Refresh Token в БД
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);

        return new LoginUserDto(
            user.Name,
            accessToken,
            refreshToken
        );
    }
}