namespace NC.AuthService.Domain;

public abstract class BaseEntity
{
    /// <summary>
    /// The unique identifier for this entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tracks the current version of the entity. 
    /// Used for concurrency checks to prevent data overwrites.
    /// </summary>
    public Guid Version { get; set; }
}