namespace NC.AuthService.Api;

// Example message for topic-based communication
public record UserUpdatedMessage(Guid UserId, string Email, DateTime UpdatedAt, string Status);