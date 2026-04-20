namespace NC.AuthService.Domain;

public class RoleClaim
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }

    public Role RoleNavigation { get; set; } = null!;
    public AppClaim AppClaimNavigation { get; set; } = null!;
}