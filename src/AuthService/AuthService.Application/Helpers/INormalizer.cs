namespace NC.AuthService.Contracts.Helpers;

public interface INormalizer
{
    public string NormalizeClaim(string value);
    (string Type, string Value) NormalizeClaims(string type, string value);
}

public static class NormalizerExtensions
{
    extension(string value)
    {
        public string NormalizeClaim(INormalizer normalizer)
        {
            return normalizer.NormalizeClaim(value);
        }
    }
}