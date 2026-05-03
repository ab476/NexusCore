namespace NC.AuthService.Contracts.Responses;

public record RoleClaimResponse
{
    public required Guid RoleId { get; init; }
    public required string ClaimType { get; init; }
    public required string ClaimValue { get; init; }
}