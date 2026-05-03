using NC.AuthService.Infrastructure.Persistence.Converters;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable($"{DbConstants.TablePrefix}role_permissions");

        // Composite Primary Key
        builder.HasKey(rc => new { rc.RoleId, rc.ClaimType, rc.ClaimValue });

        // Explicitly name the foreign key columns
        builder.Property(rc => rc.RoleId)
            .HasColumnName("role_id");

        builder.Property(rc => rc.ClaimType)
            .HasColumnName("claim_type")
            .HasConversion<LowerCaseEnumConverter<AppClaimType>>()
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(rc => rc.ClaimValue)
            .HasColumnName("claim_value")
            .HasMaxLength(256)
            .IsRequired();

        // Relationships
        builder.HasOne(rp => rp.RoleNavigation)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DbConstants.DeleteBehavior);

    }
}