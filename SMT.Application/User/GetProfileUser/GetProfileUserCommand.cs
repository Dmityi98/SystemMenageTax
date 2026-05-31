
using MediatR;
using SMT.Application.Dtos;

namespace SMT.Application.User.GetProfileUser;

public record GetProfileUserCommand(
    Guid UserId
) : IRequest<ProfileDTO>;
