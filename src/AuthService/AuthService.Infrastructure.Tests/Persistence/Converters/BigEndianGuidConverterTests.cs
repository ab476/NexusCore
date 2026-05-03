using FluentAssertions;
using NC.AuthService.Infrastructure.Persistence.Converters;

namespace NC.AuthService.Infrastructure.Tests.Persistence.Converters;

public class BigEndianGuidConverterTests
{
    private readonly BigEndianGuidConverter _converter = new();
    private readonly NullableBigEndianGuidConverter _nullableConverter = new();

    // The GUID: 0a0b0c0d-0e0f-1011-1213-141516171819

    // 1. Using the constructor overload to create a GUID from Big-Endian bytes
    private static readonly byte[] BigEndianSource = [
        0x0a, 0x0b, 0x0c, 0x0d, // Data 1
        0x0e, 0x0f,             // Data 2
        0x10, 0x11,             // Data 3
        0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19 // Data 4 (always Big-Endian)
    ];

    // This works perfectly in .NET 8+
    private static readonly Guid TestGuid = new(BigEndianSource, bigEndian: true);

    // 2. Exporting back to bytes
    private readonly byte[] ExpectedBigEndianBytes = TestGuid.ToByteArray(bigEndian: true);

    [Fact]
    public void BigEndianGuidConverter_ToBytes_ReturnsBigEndianArray()
    {
        // Act
        var result = (byte[])_converter.ConvertToProvider(TestGuid)!;

        // Assert
        result.Should().BeEquivalentTo(ExpectedBigEndianBytes, options => options.WithStrictOrdering());
    }

    [Fact]
    public void BigEndianGuidConverter_FromBytes_ReturnsCorrectGuid()
    {
        // Act
        var result = (Guid)_converter.ConvertFromProvider(ExpectedBigEndianBytes)!;

        // Assert
        result.Should().Be(TestGuid);
    }

    [Fact]
    public void NullableBigEndianGuidConverter_WithNullValue_ReturnsNull()
    {
        // Act
        var toProvider = _nullableConverter.ConvertToProvider(null);
        var fromProvider = _nullableConverter.ConvertFromProvider(null);

        // Assert
        toProvider.Should().BeNull();
        fromProvider.Should().BeNull();
    }

    [Fact]
    public void NullableBigEndianGuidConverter_WithValidValue_ConvertsCorrectly()
    {
        // Act
        var bytes = (byte[])_nullableConverter.ConvertToProvider(TestGuid)!;
        var guid = (Guid?)_nullableConverter.ConvertFromProvider(ExpectedBigEndianBytes);

        // Assert
        bytes.Should().BeEquivalentTo(ExpectedBigEndianBytes);
        guid.Should().Be(TestGuid);
    }
}