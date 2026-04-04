using FluentValidation;
using SMT.Application.UserCommand.RefreshToken;

namespace SMT.Application.Common.Validation;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh токен обязателен");
    }
}
