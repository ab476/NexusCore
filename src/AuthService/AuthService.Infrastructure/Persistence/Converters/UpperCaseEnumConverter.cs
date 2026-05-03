using Humanizer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NC.AuthService.Infrastructure.Persistence.Converters;

/// <summary>
/// Converts enums to fully UPPERCASE humanized strings in the database.
/// </summary>
/// <typeparam name="TEnum">The Enum type being converted</typeparam>
public class UpperCaseEnumConverter<TEnum> : ValueConverter<TEnum, string>
    where TEnum : struct, Enum
{
    public UpperCaseEnumConverter(ConverterMappingHints? mappingHints = null)
        : base(
            // Convert to uppercase (e.g., "PENDING PAYMENT")
            enumValue => enumValue.Humanize(LetterCasing.AllCaps),

            // Dehumanize gracefully ignores case differences when parsing back
            stringValue => stringValue.DehumanizeTo<TEnum>(),

            mappingHints)
    {
    }
}