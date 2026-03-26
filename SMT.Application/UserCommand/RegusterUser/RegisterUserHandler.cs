using System.Linq.Expressions;
using MediatR;
using SMT.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Application.User.RegusterUser;
using SMT.Domain.Exceptions;



namespace SMT.Application.UserCommand.RegusterUser;

public class RegisterUserHandler(ISMTDBContext context,  IMapper mapper, IPasswordHasher passwordHasher) :
    IRequestHandler<RegisterUserCommand, UserDTO>
{
    private readonly ISMTDBContext _context = context;

    public async Task<UserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await context.Users.AnyAsync(u => u.Name == request.Name, cancellationToken);
        
        if (userExists)
        {
            throw new NotFoundExceptions(nameof(Domain.Models.User), request.Name);
        }

        var user = new Domain.Models.User()
        {
            Name = request.Name,
            Password = passwordHasher.Generate(request.Password)
        };
        
        await _context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserDTO>(user);
    }
}