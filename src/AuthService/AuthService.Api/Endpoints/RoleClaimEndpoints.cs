using Microsoft.AspNetCore.Mvc;
using NC.AuthService.Api.Extensions;
using NC.AuthService.Contracts.Requests;
using NC.AuthService.Contracts.Services;

namespace NC.AuthService.Api.Endpoints;

public static class RoleClaimEndpoints
{
    public static void MapRoleClaimEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles")
            .WithTags("Role Claims");

        // GET: /api/roles/{roleId}/claims
        group.MapGet("/{roleId:guid}/claims", async (
            Guid roleId,
            [FromServices] IRoleClaimReadService readService) =>
        {
            var result = await readService.GetClaimsByRoleIdAsync(roleId);
            return result.ToMappedResult();
        });

        // POST: /api/roles/claims
        group.MapPost("/claims", async (
            [FromBody] AddRoleClaimRequest request,
            [FromServices] IRoleClaimWriteService writeService) =>
        {
            var result = await writeService.AddClaimToRoleAsync(request);
            return result.ToMappedResult($"/api/roles/{request.RoleId}/claims");
        });

        // POST: /api/roles/claims/bulk
        group.MapPost("/claims/bulk", async (
            [FromBody] AddRoleClaimsBulkRequest request,
            [FromServices] IRoleClaimWriteService writeService) =>
        {
            var result = await writeService.AddClaimsToRoleAsync(request);
            return result.ToMappedResult($"/api/roles/{request.RoleId}/claims");
        });

        // DELETE: /api/roles/claims
        group.MapDelete("/claims", async (
            [FromBody] RemoveRoleClaimRequest request,
            [FromServices] IRoleClaimWriteService writeService) =>
        {
            var result = await writeService.RemoveClaimFromRoleAsync(request);
            return result.ToMappedResult();
        });

        // DELETE: /api/roles/claims/bulk
        group.MapDelete("/claims/bulk", async (
            [FromBody] RemoveRoleClaimsBulkRequest request,
            [FromServices] IRoleClaimWriteService writeService) =>
        {
            var result = await writeService.RemoveClaimsFromRoleAsync(request);
            return result.ToMappedResult();
        });
    }
}
