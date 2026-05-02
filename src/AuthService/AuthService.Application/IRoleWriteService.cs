
using NC.AuthService.Abstractions.Models;
using NC.OperationResults;

namespace NC.AuthService.Abstractions;

/// <summary>
/// Handles modifying role information (Commands).
/// </summary>
public interface IRoleWriteService
{
    Task<IResult<Guid>> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    Task<IResult> UpdateAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);

    Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}