namespace NC.AuthService.Contracts.Helpers;

public class Normalizer : INormalizer
{
    public string NormalizeClaim(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return value.Trim().ToLowerInvariant();
    }

    public (string Type, string Value) NormalizeClaims(string type, string value)
    {
        return (NormalizeClaim(type), NormalizeClaim(value));
    }
}