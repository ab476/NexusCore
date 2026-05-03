using NC.AuthService.Contracts.Models;
using NC.OperationResults;

namespace NC.AuthService.Contracts;

/// <summary>
/// Handles retrieving role information (Queries).
/// </summary>
public interface IRoleReadService
{
    Task<IOutcome<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IOutcome<RoleDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IOutcome<RoleDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
