using System.Security.Claims;

namespace SMT.Application.Interfaces;
using SMT.Domain.Models;

public interface IJwtProvider
{
    string GenerateTokenAccess(List<Claim> claims = null);
    string GenerateRefreshToken();
}