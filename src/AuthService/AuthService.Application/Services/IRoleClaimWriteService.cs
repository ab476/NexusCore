using NC.AuthService.Contracts.Requests;
using NC.OperationResults;

namespace NC.AuthService.Contracts.Services;

public interface IRoleClaimWriteService
{
    Task<Outcome> AddClaimsToRoleAsync(AddRoleClaimsBulkRequest request);
    Task<Outcome> AddClaimToRoleAsync(AddRoleClaimRequest request);
    Task<Outcome> RemoveClaimFromRoleAsync(RemoveRoleClaimRequest request);
    Task<Outcome> RemoveClaimsFromRoleAsync(RemoveRoleClaimsBulkRequest request);
}
