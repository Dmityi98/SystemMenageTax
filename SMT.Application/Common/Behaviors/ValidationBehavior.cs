using FluentValidation;
using MediatR;

namespace SMT.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var errorMessages = failures.Select(f => f.ErrorMessage).ToArray();
                throw new ValidationException(errorMessages);
            }
        }

        return await next();
    }
}

public class ValidationException : Exception
{
    public string[] Errors { get; }

    public ValidationException(string[] errors) : base("Ошибка валидации")
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = Array.Empty<string>();
    }
}
