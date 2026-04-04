using FluentValidation;
using SMT.Application.Years.UpdateTable;

namespace SMT.Application.Common.Validation;

public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("ID пользователя обязателен");

        RuleFor(x => x.YearId)
            .NotEmpty().WithMessage("ID таблицы обязателен");

        RuleFor(x => x.NameTable)
            .NotEmpty().WithMessage("Название таблицы обязательно")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

        RuleFor(x => x.YearDto)
            .NotNull().WithMessage("Данные таблицы обязательны");
    }
}
