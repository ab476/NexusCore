using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable($"{DbConstants.TablePrefix}role_permissions");

        // Composite Primary Key
        builder.HasKey(rp => new { rp.RoleId, rp.ClaimId });

        // Explicitly name the foreign key columns
        builder.Property(rp => rp.RoleId).HasColumnName("role_id");
        builder.Property(rp => rp.ClaimId).HasColumnName("claim_id");

        // Relationships
        builder.HasOne(rp => rp.RoleNavigation)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DbConstants.DeleteBehavior);

        builder.HasOne(rp => rp.AppClaimNavigation)
               .WithMany(p => p.RoleClaims)
               .HasForeignKey(rp => rp.ClaimId)
               .OnDelete(DbConstants.DeleteBehavior);
    }
}