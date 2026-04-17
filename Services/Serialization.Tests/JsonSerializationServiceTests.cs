using FluentAssertions;
using MemoryPack;
using NC.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace NC.Serialization.Tests;

#region Test Models and Context

// A simple model for testing
public record TestUser(int Id, string Name);

// A Source Generation context specifically for TestUser
[JsonSerializable(typeof(TestUser))]
internal partial class TestJsonContext : JsonSerializerContext;

#endregion

public class JsonSerializationServiceTests
{
    private readonly JsonSerializationService _reflectionService = new();
    private readonly JsonSerializationService _contextService = new(TestJsonContext.Default);

    [Fact]
    public void Serialize_WithValidObject_ReturnsExpectedBytes()
    {
        // Arrange
        var user = new TestUser(1, "John Doe");

        // Act
        var result = _reflectionService.Serialize(user);
        var jsonString = Encoding.UTF8.GetString(result);

        // Assert
        result.Should().NotBeEmpty();
        jsonString.Should().Contain("\"Id\":1").And.Contain("\"Name\":\"John Doe\"");
    }

    [Fact]
    public void Deserialize_WithValidJson_ReturnsCorrectObject()
    {
        // Arrange
        const string json = "{\"Id\":99,\"Name\":\"Jane Doe\"}";
        var data = Encoding.UTF8.GetBytes(json);

        // Act
        var result = _reflectionService.Deserialize<TestUser>(data);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(99);
        result.Name.Should().Be("Jane Doe");
    }

    [Fact]
    public void Serialize_UsingContext_ProducesIdenticalResultToReflection()
    {
        // Arrange
        var user = new TestUser(42, "ContextTest");

        // Act
        var reflectionBytes = _reflectionService.Serialize(user);
        var contextBytes = _contextService.Serialize(user);

        // Assert
        contextBytes.Should().Equal(reflectionBytes);
    }

    [Fact]
    public void Serialize_NullValue_ThrowsArgumentNullException()
    {
        // Act & Assert
        var action = () => _reflectionService.Serialize<TestUser>(null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[0])]
    public void Deserialize_EmptyOrNullData_ReturnsDefault(byte[]? data)
    {
        // Act
        var result = _reflectionService.Deserialize<TestUser>(data!);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Deserialize_MalformedJson_ThrowsJsonException()
    {
        // Arrange
        var malformedData = Encoding.UTF8.GetBytes("{ \"invalid\": json ");

        // Act & Assert
        var action = () => _reflectionService.Deserialize<TestUser>(malformedData);
        action.Should().Throw<JsonException>();
    }

    [Fact]
    public void FallbackPath_WorksWhenTypeMissingFromContext()
    {
        // Arrange
        // This type is NOT in TestJsonContext
        var unregisteredModel = new { Secret = "Fallback" };

        // Act
        var result = _contextService.Serialize(unregisteredModel);
        var jsonString = Encoding.UTF8.GetString(result);

        // Assert
        jsonString.Should().Contain("Fallback");
    }

    [Fact]
    public void RoundTrip_List_MaintainsIntegrity()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var bytes = _reflectionService.Serialize(list);
        var result = _reflectionService.Deserialize<List<int>>(bytes);

        // Assert
        result.Should().HaveCount(5).And.ContainInOrder(1, 2, 3, 4, 5);
    }
}



