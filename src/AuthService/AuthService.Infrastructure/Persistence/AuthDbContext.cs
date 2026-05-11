using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata;
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

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (string.IsNullOrEmpty(tableName)) continue;

            // 1. Tables: Apply prefix and underscore
            entity.SetTableName($"{DbConstants.TablePrefix}{tableName.Underscore()}");
            var finalTableName = entity.GetTableName()!;

            // 2. Columns
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.Underscore());
            }

            // 3. Primary Keys
            // FIX: Use .SetName() instead of .SetConstraintName() for IMutableKey
            var primaryKey = entity.FindPrimaryKey();
            primaryKey?.SetName($"pk_{finalTableName}".Underscore());

            // 4. Foreign Keys
            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var colName = foreignKey.Properties.First().GetColumnName(StoreObjectIdentifier.Table(finalTableName, null));
                foreignKey.SetConstraintName($"fk_{finalTableName}_{colName}".Underscore());
            }

            // 5. Indexes
            foreach (var index in entity.GetIndexes())
            {
                var prefix = index.IsUnique ? "ux" : "ix";
                var colNames = string.Join("_", index.Properties
                    .Select(p => p.GetColumnName(StoreObjectIdentifier.Table(finalTableName, null))));

                index.SetDatabaseName($"{prefix}_{finalTableName}_{colNames}".Underscore());
            }

            // 6. Check Constraints
            // FIX: Use .Name (the property) for IMutableCheckConstraint instead of SetDatabaseName
            foreach (var checkConstraint in entity.GetCheckConstraints())
            {
                checkConstraint.Name = checkConstraint.ModelName.Underscore();
            }
        }
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
