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