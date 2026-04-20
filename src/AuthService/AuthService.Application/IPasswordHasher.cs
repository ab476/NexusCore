namespace NC.AuthService.Abstractions;

/// <summary>
/// Provides an abstraction for securely hashing and verifying passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Returns a securely hashed representation of the provided plaintext password.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>A string representing the hashed password (typically including the salt).</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies that a plaintext password matches the provided hashed password.
    /// </summary>
    /// <param name="hashedPassword">The previously stored hashed password.</param>
    /// <param name="providedPassword">The plaintext password provided during login.</param>
    /// <returns>True if the password matches; otherwise, false.</returns>
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}