using NC.AuthService.Contracts.Models;
using NC.OperationResults;

namespace NC.AuthService.Contracts;

/// <summary>
/// Handles modifying role information (Commands).
/// </summary>
public interface IRoleWriteService
{
    Task<IOutcome<Guid>> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    Task<IOutcome> UpdateAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);

    Task<IOutcome> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}