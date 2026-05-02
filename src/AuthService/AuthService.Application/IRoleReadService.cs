
using NC.AuthService.Abstractions.Models;
using NC.OperationResults;

namespace NC.AuthService.Abstractions;

/// <summary>
/// Handles retrieving role information (Queries).
/// </summary>
public interface IRoleReadService
{
    Task<IResult<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IResult<RoleDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IResult<RoleDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
