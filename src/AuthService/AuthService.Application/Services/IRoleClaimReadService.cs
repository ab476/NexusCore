using NC.AuthService.Contracts.Responses;
using NC.OperationResults;

namespace NC.AuthService.Contracts.Services;

public interface IRoleClaimReadService
{
    Task<Outcome<RoleClaimResponse[]>> GetClaimsByRoleIdAsync(Guid roleId);
}