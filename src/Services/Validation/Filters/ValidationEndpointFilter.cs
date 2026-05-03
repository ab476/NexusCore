using Microsoft.AspNetCore.Http;
using NC.Validation.Abstractions;

namespace NC.Validation.Filters;

public class ValidationEndpointFilter<TModel>(IValidationService vs) : IEndpointFilter
{
    private readonly IValidationService _validationService = vs;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var arg in context.Arguments)
        {
            if (arg is not TModel instance) continue;

            var cancellationToken = context.HttpContext.RequestAborted;

            var result = await _validationService.ValidateAsync(instance, context.HttpContext.RequestServices, cancellationToken);
            if (!result.IsValid) return Results.BadRequest(result);
        }
        return await next(context);
    }
}