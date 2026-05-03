using NC.AuthService.Infrastructure.Persistence.Configurations;
using NC.AuthService.Infrastructure.Persistence.Converters;

namespace NC.AuthService.Infrastructure.Persistence;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<SecurityToken> RefreshTokens => Set<SecurityToken>();

    // Join tables (Optional to expose as DbSets, but useful for direct querying)
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Guid>()
            .HaveConversion<BigEndianGuidConverter>();

        configurationBuilder
            .Properties<Guid?>()
            .HaveConversion<NullableBigEndianGuidConverter>();

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
    internal static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new SecurityTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
    }
}
