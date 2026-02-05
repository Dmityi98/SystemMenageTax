using System.Linq.Expressions;
using MediatR;
using SMT.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Application.User.RegisterUser;
using SMT.Domain.Exceptions;



namespace SMT.Application.UserCommand.RegisterUser;

public class RegisterUserHandler(ISMTDBContext context,  IMapper mapper, IPasswordHasher passwordHasher) :
    IRequestHandler<RegisterUserCommand, RegisterUserDTO>
{

    public async Task<RegisterUserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
        
        if (user != null)
        {
            throw new NotFoundExceptions(nameof(User), request.Name);
        }

        user = new Domain.Models.User()
        {
            Name = request.Name,
            Password = passwordHasher.Generate(request.Password)
        };
        
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<RegisterUserDTO>(user);
    }
}