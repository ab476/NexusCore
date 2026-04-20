using FluentAssertions;
using MemoryPack;

namespace NC.Serialization.Tests;

#region Test Models

[MemoryPackable]
public partial class ValidModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<string> Tags { get; set; } = [];
}

[MemoryPackable]
public partial class NestedModel
{
    public Guid OrderId { get; set; }
    public ValidModel User { get; set; } = null!;
}

// This model is NOT MemoryPackable - used for failure testing
public class NonPackableModel
{
    public int Id { get; set; }
}

#endregion


public class MemoryPackSerializationServiceTests
{
    private readonly MemoryPackSerializationService _service = new();



    [Fact]
    public void Serialize_ValidObject_ReturnsNonEmptyBinary()
    {
        // Arrange
        var model = new ValidModel { Id = 1, Name = "MemoryPack", Tags = ["Fast", "Binary"] };

        // Act
        var result = _service.Serialize(model);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void RoundTrip_ValidObject_MaintainsDataIntegrity()
    {
        // Arrange
        var original = new ValidModel { Id = 500, Name = "IntegrityCheck" };

        // Act
        var bytes = _service.Serialize(original);
        var deserialized = _service.Deserialize<ValidModel>(bytes);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Id.Should().Be(original.Id);
        deserialized.Name.Should().Be(original.Name);
    }

    [Fact]
    public void RoundTrip_NestedComplexObject_Succeeds()
    {
        // Arrange
        var original = new NestedModel
        {
            OrderId = Guid.NewGuid(),
            User = new ValidModel { Id = 1, Name = "Nested" }
        };

        // Act
        var bytes = _service.Serialize(original);
        var deserialized = _service.Deserialize<NestedModel>(bytes);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.OrderId.Should().Be(original.OrderId);
        deserialized.User.Name.Should().Be("Nested");
    }

    [Fact]
    public void Serialize_NullValue_ThrowsArgumentNullException()
    {
        // Act & Assert
        var action = () => _service.Serialize<ValidModel>(null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[0])]
    public void Deserialize_EmptyOrNullData_ReturnsDefault(byte[]? data)
    {
        // Act
        var result = _service.Deserialize<ValidModel>(data!);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Serialize_NonPackableType_ThrowsException()
    {
        // Arrange
        var invalidModel = new NonPackableModel { Id = 1 };

        // Act & Assert
        // MemoryPack source generator usually prevents this at compile time, 
        // but if passed through a generic interface, it may throw at runtime.
        var action = () => _service.Serialize(invalidModel);
        action.Should().Throw<Exception>();
    }
}