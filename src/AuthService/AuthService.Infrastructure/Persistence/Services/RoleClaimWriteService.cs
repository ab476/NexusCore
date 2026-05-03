using NC.AuthService.Contracts.Helpers;

namespace NC.AuthService.Infrastructure.Persistence.Services;

public class RoleClaimWriteService(AuthDbContext _context) : IRoleClaimWriteService
{
    public async Task<Outcome> AddClaimToRoleAsync(AddRoleClaimRequest request)
    {
        var roleExists = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
        if (!roleExists)
        {
            return Outcome.Failure("RoleNotFound",
                $"Role with ID {request.RoleId} does not exist.",
                HttpStatusCode.NotFound);
        }

        var alreadyHasClaim = await _context.RoleClaims.AnyAsync(rc =>
            rc.RoleId == request.RoleId &&
            rc.ClaimType == request.ClaimType &&
            rc.ClaimValue == request.ClaimValue);

        if (alreadyHasClaim)
        {
            return Outcome.Failure("DuplicateClaim",
                "This role already possesses this specific claim.",
                HttpStatusCode.Conflict);
        }

        var newClaim = new RoleClaim
        {
            RoleId = request.RoleId,
            ClaimType = request.ClaimType,
            ClaimValue = request.ClaimValue
        };

        _context.RoleClaims.Add(newClaim);
        await _context.SaveChangesAsync();

        return Outcome.Success("Claim added successfully.", HttpStatusCode.Created);
    }

    public async Task<Outcome> AddClaimsToRoleAsync(AddRoleClaimsBulkRequest request)
    {
        var roleExists = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
        if (!roleExists)
        {
            return Outcome.Failure("RoleNotFound", $"Role with ID {request.RoleId} does not exist.", HttpStatusCode.NotFound);
        }

        var existingClaimsSet = (await _context.RoleClaims
            .Where(rc => rc.RoleId == request.RoleId)
            .Select(rc => new { rc.ClaimType, rc.ClaimValue })
            .ToListAsync())
            .Select(x => (x.ClaimType, x.ClaimValue))
            .ToHashSet();

        var newEntities = new List<RoleClaim>();

        foreach (var item in request.Claims)
        {
            if (!existingClaimsSet.Contains((item.ClaimType, item.ClaimValue)))
            {
                newEntities.Add(new RoleClaim
                {
                    RoleId = request.RoleId,
                    ClaimType = item.ClaimType,
                    ClaimValue = item.ClaimValue
                });
            }
        }

        if (newEntities.Count == 0)
        {
            return Outcome.Success("No new unique claims to add.", HttpStatusCode.OK);
        }

        _context.RoleClaims.AddRange(newEntities);
        await _context.SaveChangesAsync();

        return Outcome.Success($"{newEntities.Count} claims added successfully.", HttpStatusCode.Created);
    }

    public async Task<Outcome> RemoveClaimFromRoleAsync(RemoveRoleClaimRequest request)
    {
        var claim = await _context.RoleClaims.FirstOrDefaultAsync(rc =>
            rc.RoleId == request.RoleId &&
            rc.ClaimType == request.ClaimType &&
            rc.ClaimValue == request.ClaimValue);

        if (claim is null)
        {
            return Outcome.Failure("ClaimNotFound",
                "The specified claim was not found on this role.",
                HttpStatusCode.NotFound);
        }

        _context.RoleClaims.Remove(claim);
        await _context.SaveChangesAsync();

        return Outcome.Success("Claim removed successfully.", HttpStatusCode.NoContent);
    }

    public async Task<Outcome> RemoveClaimsFromRoleAsync(RemoveRoleClaimsBulkRequest request)
    {
        var claimsToRemove = await _context.RoleClaims
            .Where(rc => rc.RoleId == request.RoleId)
            .ToListAsync();

        var filteredToRemove = claimsToRemove
            .Where(rc => request.Claims.Any(c =>
                c.ClaimType == rc.ClaimType && c.ClaimValue == rc.ClaimValue))
            .ToList();

        if (filteredToRemove.Count == 0)
        {
            return Outcome.Failure("NoClaimsFound", "None of the specified claims were found on this role.", HttpStatusCode.NotFound);
        }

        _context.RoleClaims.RemoveRange(filteredToRemove);
        await _context.SaveChangesAsync();

        return Outcome.Success($"{filteredToRemove.Count} claims removed successfully.", HttpStatusCode.NoContent);
    }
}