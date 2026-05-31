
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SMT.Application.Dtos;
using SMT.Application.Interfaces;
using SMT.Domain.Exceptions;
using SMT.Domain.Models;

namespace SMT.Application.User.GetProfileUser;

public class GetUserProfileHandler(
    ISMTDBContext context,
    IMapper mapper) : IRequestHandler<GetProfileUserCommand, ProfileDTO>
{
    public async Task<ProfileDTO> Handle(GetProfileUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundExceptions(nameof(UserProfile), request.UserId);
        }

        return mapper.Map<ProfileDTO>(entity);
    }
}