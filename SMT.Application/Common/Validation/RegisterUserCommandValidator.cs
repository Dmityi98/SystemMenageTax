using FluentValidation;
using SMT.Application.UserCommand.RegisterUser;

namespace SMT.Application.Common.Validation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя пользователя обязательно")
            .MinimumLength(3).WithMessage("Имя должно содержать минимум 3 символа")
            .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов")
            .Matches(@"^[a-zA-Zа-яА-Я0-9_]+$").WithMessage("Имя может содержать только буквы, цифры и подчеркивание");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов")
            .MaximumLength(100).WithMessage("Пароль не должен превышать 100 символов")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");
    }
}
