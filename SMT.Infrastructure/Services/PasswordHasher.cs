using SMT.Application.Interfaces;

namespace SMT.Infrastructure.Services;

public class PasswordHasher: IPasswordHasher
{

    public string Generate(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHeshed) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHeshed);

}