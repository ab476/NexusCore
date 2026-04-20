namespace NC.AuthService.Abstractions.Models;

/// <summary>
/// Represents the data required to register a new user.
/// </summary>
public record RegisterRequest
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}

/// <summary>
/// Represents the data required to authenticate an existing user.
/// </summary>
public record LoginRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}