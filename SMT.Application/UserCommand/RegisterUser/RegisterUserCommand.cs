using MediatR;

namespace SMT.Application.UserCommand.RegisterUser;

/// <summary>
/// Команда регистрации нового пользователя
/// </summary>
public record RegisterUserCommand(
    string Name,
    string Password
) : IRequest<RegisterUserDto>;