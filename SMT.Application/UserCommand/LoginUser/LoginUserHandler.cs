using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.UserCommand.LoginUser;

public class LoginUserCommandHandler(
    IMapper mapper,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    ISMTDBContext contexts) :
    IRequestHandler<LoginUserCommand, LoginUserDTO>
{
    public async Task<LoginUserDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await contexts.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
        
        if (user == null)
        {
              throw new Exception();
        }
        
        if (!passwordHasher.Verify(request.Password, user.Password))
        {
            throw new Exception();
        }
        
        return new LoginUserDTO(
            user.Name,
            jwtProvider.GenerateToken(user));
    }
}