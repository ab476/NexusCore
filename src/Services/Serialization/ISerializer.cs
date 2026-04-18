namespace NC.Serialization;

/// <summary>
/// Defines a contract for binary serialization and deserialization services.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Serializes the specified object into a byte array.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A byte array representation of the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    byte[] Serialize<T>(T value);

    /// <summary>
    /// Deserializes a byte array back into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="data">The byte array to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>, or null if deserialization fails.</returns>
    T? Deserialize<T>(byte[] data);
}
