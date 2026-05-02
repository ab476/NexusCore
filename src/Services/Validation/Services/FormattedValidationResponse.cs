namespace NC.Validation.Services;

public sealed class FormattedValidationResponse
{
    public bool IsValid => Errors.Count == 0;

    public List<ValidationError> Errors { get; init; } = [];
}
