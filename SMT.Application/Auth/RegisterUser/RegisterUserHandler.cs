
using MediatR;
using SMT.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Domain.Models;
using SMT.Domain.Exceptions;

namespace SMT.Application.UserCommand.RegisterUser;

/// <summary>
/// Обработчик команды регистрации пользователя
/// </summary>
public class RegisterUserHandler(
    ISMTDBContext context,
    IMapper mapper,
    IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, RegisterUserDto>
{
    public async Task<RegisterUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Проверка: существует ли пользователь с таким именем
        var existingUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

        if (existingUser != null)
        {
            throw new ConflictException($"Пользователь с именем '{request.Name}' уже существует");
        }

        // Создание нового пользователя
        var user = new Domain.Models.User
        {
            Name = request.Name,
            Password = passwordHasher.Generate(request.Password),
        };
        user.Profile = UserProfile.Create(user.Id);
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<RegisterUserDto>(user);
    }
}