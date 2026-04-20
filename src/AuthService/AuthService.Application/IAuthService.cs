using NC.AuthService.Abstractions.Models;

namespace NC.AuthService.Abstractions;

/// <summary>
/// Defines the contract for the authentication service, providing core authentication functionalities.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided credentials.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>An authentication result indicating success or failure, and potentially a token.</returns>
    Task<AuthResult> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <returns>An authentication result indicating success or failure, and potentially a token.</returns>
    Task<AuthResult> LoginAsync(LoginRequest request);

    /// <summary>
    /// Validates an authentication token.
    /// </summary>
    /// <param name="token">The token string to validate.</param>
    /// <returns>True if the token is valid, false otherwise.</returns>
    Task<bool> ValidateTokenAsync(string token);
}
