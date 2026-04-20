using System.Net;

namespace NC.AuthService.Domain;

/// <summary>
/// Defines the purpose of the security token.
/// </summary>
public enum TokenType
{
    RefreshToken,
    AccountActivation,
    PasswordReset,
    EmailVerification
}

public class SecurityToken : BaseEntity
{
    // Foreign Key
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    /// <summary>
    /// Identifies what this token is authorized to do.
    /// </summary>
    public TokenType Type { get; set; }

    /// <summary>
    /// The securely hashed version of the token.
    /// The plaintext token should be returned to the user ONCE upon creation and never stored.
    /// </summary>
    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    // --- Audit & Rotation Fields ---
    // These are primarily used for RefreshTokens, but can be left nullable for short-lived tokens.

    /// <summary>
    /// Used to detect token reuse/theft. Stores the hash of the token that replaced this one.
    /// </summary>
    public Guid? ReplacedById { get; set; }

    public IPAddress? RevokedByIp { get; set; }

    // --- Helper Computed Properties ---
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}