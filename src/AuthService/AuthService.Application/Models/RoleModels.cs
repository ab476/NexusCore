using NC.AuthService.Domain;

namespace NC.AuthService.Abstractions.Models;

public record RoleDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public static implicit operator RoleDto(Role role) =>
        new()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
        };
}

public record CreateRoleRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public record UpdateRoleRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
