namespace NC.AuthService.Domain;

/// <summary>
/// Defines the allowed types of claims within the system.
/// </summary>
public enum AppClaimType
{
    Permission,
    Department,
    TenantId,
    JobTitle,
    ManagerId
}

/// <summary>
/// Represents a system-defined claim that can be assigned to roles (or users).
/// </summary>
public class AppClaim : BaseEntity
{
    /// <summary>
    /// The type of the claim, strongly typed.
    /// </summary>
    public required AppClaimType ClaimType { get; set; }

    /// <summary>
    /// The specific value of the claim (e.g., "users.create", "HR", "1042").
    /// </summary>
    public required string ClaimValue { get; set; }

    /// <summary>
    /// A human-readable description for UI administration.
    /// </summary>
    public required string Description { get; set; }

    // Navigation Properties
    public ICollection<RoleClaim> RoleClaims { get; set; } = [];
}