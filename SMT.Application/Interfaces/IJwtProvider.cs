using SMT.Domain.Models;

namespace SMT.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(Domain.Models.User user);
}