using MediatR;

namespace SMT.Application.User.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserDTO>
{
    public string Name { get; set; }
    public string Password { get; set; }
}