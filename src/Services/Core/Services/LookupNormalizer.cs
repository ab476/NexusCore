using Core.Abstractions;
using System.Text.RegularExpressions;

namespace Core.Services;

public partial class LookupNormalizer : ILookupNormalizer
{
    [GeneratedRegex(@"[^A-Z0-9_\-\.]")]
    private static partial Regex InvalidCharactersRegex();

    public string Normalize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        string normalized = name.Trim().ToUpperInvariant();

        normalized = normalized.Replace(" ", "_");

        // Aggressively strip out any remaining invalid characters
        // Example: "Admin@123!" becomes "ADMIN123"
        normalized = InvalidCharactersRegex().Replace(normalized, "");

        return normalized;
    }
}