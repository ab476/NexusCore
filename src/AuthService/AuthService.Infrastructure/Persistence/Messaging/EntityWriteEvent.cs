namespace NC.AuthService.Infrastructure.Persistence.Messaging;
public record EntityWriteEvent<T> where T : class
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();
    public required string EntityName { get; init; }
    public required EntityOperation Operation { get; init; }
    public required DateTime OccurredAtUtc { get; init; }

    public required T Data { get; init; }
}

public enum EntityOperation
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3
}