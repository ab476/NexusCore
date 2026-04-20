namespace NC.AuthService.Api;

// Example message for queue-based communication
public record UserCreatedMessage(Guid UserId, string Email, DateTime CreatedAt);