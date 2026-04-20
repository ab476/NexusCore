namespace NC.AuthService.Abstractions;

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
public record AuthResult(
    bool Success,
    string? Token = null,
    string? ErrorMessage = null
);