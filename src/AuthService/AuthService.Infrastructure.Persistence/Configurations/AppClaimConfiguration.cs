using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;
public class AppClaimConfiguration : BaseEntityConfiguration<AppClaim>
{
    public new void Configure(EntityTypeBuilder<AppClaim> builder)
    {
        base.Configure(builder, "app_claims");

        builder.Property(c => c.ClaimType)
               .HasColumnName("claim_type")
               .HasConversion<int>()
               .IsRequired();

        builder.Property(c => c.ClaimValue)
               .HasColumnName("claim_value")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(c => c.Description)
               .HasColumnName("description")
               .HasMaxLength(250)
               .IsRequired();

        builder.HasIndex(c => new { c.ClaimType, c.ClaimValue })
               .HasDatabaseName($"ix_{TablePrefix}app_claims__claim_type__claim_value")
               .IsUnique();
    }
}