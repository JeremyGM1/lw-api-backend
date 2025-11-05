using FluentValidation;
using MediatR;
using Application.Common.Results;
using Application.Common.Results.GenericErrors;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validatorList = _validators.ToList();

        if (validatorList.Count == 0)
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var result = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = result.Where(r => !r.IsValid).SelectMany(r => r.Errors).ToList();

        if (failures.Count != 0)
        {
            var errorMessage = string.Join(", ", failures.Select(f => f.ErrorMessage));
            var error = ValidationErrors.InvalidInput(errorMessage);

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var method = typeof(TResponse).GetMethod("Failure", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                return (TResponse)method!.Invoke(null, [error])!;
            }
            else
            {
                return (TResponse)(object)Result.Failure(error);
            }
        }

        return await next(cancellationToken);
    }
}