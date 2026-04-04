using MediatR;

namespace SMT.Application.UserCommand.LoginUser;

/// <summary>
/// Команда входа пользователя
/// </summary>
public record LoginUserCommand(
    string Name,
    string Password
) : IRequest<LoginUserDto>;