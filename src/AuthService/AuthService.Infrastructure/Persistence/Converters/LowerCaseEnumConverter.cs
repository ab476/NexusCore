using Humanizer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NC.AuthService.Infrastructure.Persistence.Converters;

/// <summary>
/// Converts enums to fully lowercase humanized strings in the database.
/// </summary>
/// <typeparam name="TEnum">The Enum type being converted</typeparam>
public class LowerCaseEnumConverter<TEnum>(ConverterMappingHints? mappingHints = null) : ValueConverter<TEnum, string>(
            // Convert to lowercase (e.g., "pending payment")
        enumValue => enumValue.Humanize(LetterCasing.LowerCase),

            // Dehumanize gracefully ignores case differences when parsing back
        stringValue => stringValue.DehumanizeTo<TEnum>(),

        mappingHints)
    where TEnum : struct, Enum
{
}
