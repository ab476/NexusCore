using System;
using System.Collections.Generic;
using System.Text;

namespace NC.AuthService.Contracts.Requests;

public record AddRoleClaimRequest
{
    public required Guid RoleId { get; init; }
    public required string ClaimType { get; init; }
    public required string ClaimValue { get; init; }
}

public record RemoveRoleClaimRequest
{
    public required Guid RoleId { get; init; }
    public required string ClaimType { get; init; }
    public required string ClaimValue { get; init; }
}

public record AddRoleClaimsBulkRequest
{
    public required Guid RoleId { get; init; }
    public required List<RoleClaimItem> Claims { get; init; }
}

public record RemoveRoleClaimsBulkRequest
{
    public required Guid RoleId { get; init; }
    public required List<RoleClaimItem> Claims { get; init; }
}

public record RoleClaimItem
{
    public required string ClaimType { get; init; }
    public required string ClaimValue { get; init; }
}