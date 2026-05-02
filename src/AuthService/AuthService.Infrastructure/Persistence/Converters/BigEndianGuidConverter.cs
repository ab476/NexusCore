using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NC.AuthService.Infrastructure.Persistence.Converters;

/// <summary>
/// Converts a <see cref="Guid"/> to a 16-byte array in Big-Endian format and vice versa.
/// </summary>
/// <remarks>
/// This is particularly useful for databases or external systems that expect Big-Endian 
/// byte ordering (RFC 4122) rather than the Little-Endian format natively used by .NET on Windows.
/// Requires .NET 8.0+.
/// </remarks>
public class BigEndianGuidConverter : ValueConverter<Guid, byte[]>
{
    private static readonly ConverterMappingHints _defaultHints = new(size: 16);

    /// <summary>
    /// Initializes a new instance of the <see cref="BigEndianGuidConverter"/> class.
    /// </summary>
    public BigEndianGuidConverter()
        : base(
            v => v.ToByteArray(bigEndian: true),
            v => new Guid(v, bigEndian: true),
            _defaultHints)
    {
    }
}
