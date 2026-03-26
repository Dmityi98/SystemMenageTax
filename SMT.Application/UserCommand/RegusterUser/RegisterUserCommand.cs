using MediatR;

namespace SMT.Application.User.RegusterUser;

public class RegisterUserCommand : IRequest<UserDTO>
{
    public string Name { get; set; }
    public string Password { get; set; }
}