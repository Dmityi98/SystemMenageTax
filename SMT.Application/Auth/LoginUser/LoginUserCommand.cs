using MediatR;

namespace SMT.Application.Auth.LoginUser;

/// <summary>
/// Команда входа пользователя
/// </summary>
public record LoginUserCommand(
    string Name,
    string Password
) : IRequest<LoginUserDto>;