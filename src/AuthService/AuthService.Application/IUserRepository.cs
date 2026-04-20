
namespace NC.AuthService.Abstractions;

/// <summary>
/// Defines the contract for user data access operations.
/// This interface abstracts the persistence layer for user entities.
/// </summary>
/// <typeparam name="TUser">The type of the user entity, typically from the domain layer.</typeparam>
public interface IUserRepository<TUser> where TUser : class
{
    /// <summary>
    /// Finds a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user entity if found, otherwise null.</returns>
    Task<TUser?> FindByIdAsync(Guid userId);

    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user entity if found, otherwise null.</returns>
    Task<TUser?> FindByEmailAsync(string email);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>The added user entity.</returns>
    Task<TUser> AddAsync(TUser user);
}