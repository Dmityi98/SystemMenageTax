using MediatR;

namespace SMT.Application.UserCommand.LoginUser;

public class LoginUserCommand :IRequest<LoginUserDTO>
{
    public string Name { get; set; }
    public string Password { get; set; }
}