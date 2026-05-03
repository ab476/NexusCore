using Humanizer;

namespace NC.AuthService.Infrastructure.Persistence.Services;

public class RoleClaimReadService(AuthDbContext context) : IRoleClaimReadService
{
    public async Task<Outcome<RoleClaimResponse[]>> GetClaimsByRoleIdAsync(Guid roleId)
    {
        var claims = await context.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .AsNoTracking()
            .Select(x => new RoleClaimResponse
            {
                RoleId = x.RoleId,
                ClaimType = x.ClaimType.Humanize(),
                ClaimValue = x.ClaimValue,
            })
            .ToArrayAsync();

        return Outcome.Success(claims, "Claims retrieved successfully.", HttpStatusCode.OK);
    }
}