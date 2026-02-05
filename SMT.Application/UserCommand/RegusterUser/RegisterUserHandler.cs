using System.Linq.Expressions;
using MediatR;
using SMT.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Application.User.RegusterUser;
using SMT.Domain.Exceptions;



namespace SMT.Application.UserCommand.RegusterUser;

public class RegisterUserHandler(ISMTDBContext context,  IMapper mapper) :
    IRequestHandler<RegisterUserCommand, UserDTO>
{
    private readonly ISMTDBContext _context = context;

    public async Task<UserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
        
        if (user != null)
        {
            throw new NotFoundExceptions(nameof(User), request.Name);
        }

        user = new Domain.Models.User()
        {
            Name = request.Name,
            Password = request.Password
        };
        
        await _context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserDTO>(user);
    }
}