using MemoryPack;

namespace NC.Serialization;

/// <summary>
/// Provides ultra-fast binary serialization using the MemoryPack library.
/// </summary>
/// <remarks>
/// Types passed to this service must be decorated with the [MemoryPackable] attribute.
/// </remarks>
public class MemoryPackSerializationService : ISerializer
{
    /// <inheritdoc />
    /// <exception cref="MemoryPackSerializationException">Thrown if the type T is not MemoryPackable.</exception>
    public byte[] Serialize<T>(T value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        return MemoryPackSerializer.Serialize(value);
    }

    /// <inheritdoc />
    public T? Deserialize<T>(byte[] data)
    {
        if (data == null || data.Length == 0) return default;
        return MemoryPackSerializer.Deserialize<T>(data);
    }
}
