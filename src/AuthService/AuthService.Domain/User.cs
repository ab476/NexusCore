namespace NC.AuthService.Domain;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool IsEmailVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // For account lockouts (Brute Force Protection)
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<SecurityToken> SecurityTokens { get; set; } = [];
}
