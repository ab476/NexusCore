using Microsoft.EntityFrameworkCore;
using NC.AuthService.Domain;
using NC.AuthService.Infrastructure.Persistence.Configurations;

namespace NC.AuthService.Infrastructure.Persistence;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<AppClaim> Permissions => Set<AppClaim>();
    public DbSet<SecurityToken> RefreshTokens => Set<SecurityToken>();

    // Join tables (Optional to expose as DbSets, but useful for direct querying)
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RoleClaim> RolePermissions => Set<RoleClaim>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Guid>()
            .HaveConversion<BigEndianGuidConverter>();

        base.ConfigureConventions(configurationBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplyEntityConfigurations(modelBuilder);
    }

    /// <summary>
    /// Applies all explicit Entity Framework entity configurations.
    /// </summary>
    private static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new AppClaimConfiguration());
        modelBuilder.ApplyConfiguration(new SecurityTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
    }
}
