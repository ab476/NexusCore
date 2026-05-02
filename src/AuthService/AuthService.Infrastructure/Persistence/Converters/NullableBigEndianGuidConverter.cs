using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NC.AuthService.Infrastructure.Persistence.Converters;

/// <summary>
/// Converts a nullable <see cref="Guid"/> to a nullable 16-byte array in Big-Endian format and vice versa.
/// </summary>
/// <remarks>
/// Provides explicit handling for null values while maintaining Big-Endian byte ordering 
/// for the underlying <see cref="Guid"/> data. Requires .NET 8.0+.
/// </remarks>
public class NullableBigEndianGuidConverter : ValueConverter<Guid?, byte[]?>
{
    private static readonly ConverterMappingHints _defaultHints = new(size: 16);

    /// <summary>
    /// Initializes a new instance of the <see cref="NullableBigEndianGuidConverter"/> class.
    /// </summary>
    public NullableBigEndianGuidConverter()
        : base(
            v => v.HasValue ? v.Value.ToByteArray(bigEndian: true) : null,
            v => v != null ? new Guid(v, bigEndian: true) : null,
            _defaultHints)
    {
    }
}