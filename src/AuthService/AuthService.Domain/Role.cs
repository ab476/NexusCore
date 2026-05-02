namespace NC.AuthService.Domain;

public class Role : BaseEntity
{
    public required string Name { get; set; } // e.g., "Admin", "Manager", "User"
    /// <summary>
    /// The normalized version of the name (usually UPPERCASE) used for consistent,
    /// case-insensitive database lookups and unique constraints.
    /// </summary>
    public required string NormalizedName { get; set; }
    public required string Description { get; set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<RoleClaim> RoleClaims { get; set; } = [];
}
