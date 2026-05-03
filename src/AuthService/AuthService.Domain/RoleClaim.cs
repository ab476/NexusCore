namespace NC.AuthService.Domain;

public class RoleClaim
{
    public required Guid RoleId { get; set; }
    public required string ClaimType { get; set; }
    public required string ClaimValue { get; set; }

    public Role RoleNavigation { get; set; } = null!;
}