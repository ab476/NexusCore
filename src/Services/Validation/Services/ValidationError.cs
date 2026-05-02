namespace NC.Validation.Services;

public sealed class ValidationError
{
    public string Property { get; init; } = default!;

    public string Message { get; init; } = default!;
}