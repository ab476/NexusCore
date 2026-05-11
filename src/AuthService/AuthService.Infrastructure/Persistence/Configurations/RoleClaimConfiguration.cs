using NC.AuthService.Infrastructure.Persistence.Converters;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {

        builder.HasKey(rc => new { rc.RoleId, rc.ClaimType, rc.ClaimValue });

        builder.Property(rc => rc.ClaimType)
            .HasConversion<LowerCaseEnumConverter<AppClaimType>>()
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(rc => rc.ClaimValue)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasOne(rp => rp.RoleNavigation)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DbConstants.DeleteBehavior);
    }
}