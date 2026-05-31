using FluentValidation;
using SMT.Application.Years.CreateTable;

namespace SMT.Application.Common.Validation;

public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(x => x.NameTable)
            .NotEmpty().WithMessage("Название таблицы обязательно")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");
    }
}