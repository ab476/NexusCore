using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NC.Serialization;

/// <summary>
/// Provides JSON serialization using System.Text.Json with support for Source Generated contexts.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonSerializationService"/> class.
/// </remarks>
/// <param name="context">An optional <see cref="JsonSerializerContext"/> to enable Source Generation performance benefits.</param>
public class JsonSerializationService(JsonSerializerContext? context = null) : ISerializer
{
    private readonly JsonSerializerContext? _context = context;

    /// <inheritdoc />
    public byte[] Serialize<T>(T value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        // Attempt to get specialized TypeInfo from the context for better performance/AOT safety
        var typeInfo = _context?.GetTypeInfo(typeof(T)) as JsonTypeInfo<T>;

        return typeInfo is not null
            ? JsonSerializer.SerializeToUtf8Bytes(value, typeInfo)
            : JsonSerializer.SerializeToUtf8Bytes(value);
    }

    /// <inheritdoc />
    public T? Deserialize<T>(byte[] data)
    {
        if (data == null || data.Length == 0) return default;

        var typeInfo = _context?.GetTypeInfo(typeof(T)) as JsonTypeInfo<T>;

        return typeInfo is not null
            ? JsonSerializer.Deserialize(data, typeInfo)
            : JsonSerializer.Deserialize<T>(data);
    }
}
