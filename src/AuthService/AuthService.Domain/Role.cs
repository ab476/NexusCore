namespace NC.AuthService.Domain;

public class Role : BaseEntity
{
    public required string Name { get; set; } // e.g., "Admin", "Manager", "User"
    public required string Description { get; set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<RoleClaim> RoleClaims { get; set; } = [];
}
