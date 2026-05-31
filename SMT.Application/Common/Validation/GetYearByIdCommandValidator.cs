using FluentValidation;
using SMT.Application.Years.GetYearById;

namespace SMT.Application.Common.Validation;

public class GetYearByIdCommandValidator : AbstractValidator<GetYearByIdCommand>
{
    public GetYearByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID таблицы обязателен");
    }
}