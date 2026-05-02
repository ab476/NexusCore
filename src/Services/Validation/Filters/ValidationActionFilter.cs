using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NC.Validation.Abstractions;

namespace NC.Validation.Filters;

public sealed class ValidationActionFilter(IValidationService validationService) : IAsyncActionFilter
{
    private readonly IValidationService _validationService = validationService;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        ICollection<object?> args = context.ActionArguments.Values;
        var cancellationToken = context.HttpContext.RequestAborted;
        var serviceProvider = context.HttpContext.RequestServices;

        var result = await _validationService.ValidateAsync(args, serviceProvider, cancellationToken);

        if (!result.IsValid)
        {
            context.Result = new BadRequestObjectResult(result);
            return;
        }

        await next();
    }
}