using Core.Abstractions;
using NC.AuthService.Contracts;
using NC.AuthService.Contracts.Models;

namespace NC.AuthService.Infrastructure.Persistence.Services;

public class RoleWriteService(AuthDbContext context, ILookupNormalizer normalizer) : IRoleWriteService
{
    public async Task<IOutcome<Guid>> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        string normalizedName = normalizer.Normalize(request.Name);

        bool roleExists = await context.Roles
            .AnyAsync(r => r.NormalizedName == normalizedName, cancellationToken);

        if (roleExists)
        {
            return Outcome.Failure<Guid>(
                error: "RoleNameConflict",
                message: $"A role with the name '{request.Name}' already exists.",
                statusCode: HttpStatusCode.Conflict);
        }

        var newRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NormalizedName = normalizedName,
            Description = request.Description
        };

        context.Roles.Add(newRole);
        await context.SaveChangesAsync(cancellationToken);

        return Outcome.Success(newRole.Id, message: "Role created successfully.", statusCode: HttpStatusCode.Created);
    }

    public async Task<IOutcome> UpdateAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var existingRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (existingRole == null)
        {
            return Outcome.Failure(
                error: "RoleNotFound",
                message: "The role you are trying to update does not exist.",
                statusCode: HttpStatusCode.NotFound);
        }

        string normalizedName = normalizer.Normalize(request.Name);

        if (!string.Equals(existingRole.Name, request.Name, StringComparison.OrdinalIgnoreCase))
        {
            bool nameTaken = await context.Roles
                .AnyAsync(r => r.NormalizedName == normalizedName && r.Id != request.Id, cancellationToken);

            if (nameTaken)
            {
                return Outcome.Failure(
                    error: "RoleNameConflict",
                    message: "Another role is already using this name.",
                    statusCode: HttpStatusCode.Conflict);
            }
        }

        existingRole.Name = request.Name;
        existingRole.Description = request.Description;
        existingRole.NormalizedName = normalizedName;

        context.Roles.Update(existingRole);
        await context.SaveChangesAsync(cancellationToken);

        return Outcome.Success(message: "Role updated successfully.", statusCode: HttpStatusCode.OK);
    }

    public async Task<IOutcome> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (existingRole == null)
        {
            return Outcome.Failure(
                error: "RoleNotFound",
                message: "The role you are trying to delete does not exist.",
                statusCode: HttpStatusCode.NotFound);
        }

        context.Roles.Remove(existingRole);
        await context.SaveChangesAsync(cancellationToken);

        return Outcome.Success(message: "Role deleted successfully.", statusCode: HttpStatusCode.NoContent);
    }
}