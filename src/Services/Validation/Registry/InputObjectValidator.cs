using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NC.Validation.Registry;

internal class InputObjectValidator<T>(IValidator<T> validator) : IInputObjectValidator
{
    public async Task<ValidationResult?> ValidateAsync(object instance, CancellationToken cancellationToken)
    {
        if (instance is not T input)
        {
            return null;
        }

        return await validator.ValidateAsync(input, cancellationToken);
    }
}
