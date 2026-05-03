namespace NC.AuthService.Domain;

public abstract class BaseEntity
{
    /// <summary>
    /// The unique identifier for this entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Points to the Id of the previous version of this entity.
    /// </summary>
    public Guid? LastVersionId { get; set; }

    /// <summary>
    /// Tracks the current version of the entity. 
    /// Used for concurrency checks to prevent data overwrites.
    /// </summary>
    public Guid VersionId { get; set; }
}