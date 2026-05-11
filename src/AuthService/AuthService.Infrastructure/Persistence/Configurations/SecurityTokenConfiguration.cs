using NC.AuthService.Infrastructure.Persistence.Converters;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class SecurityTokenConfiguration : BaseEntityConfiguration<SecurityToken>
{
    public new void Configure(EntityTypeBuilder<SecurityToken> builder)
    {
        base.Configure(builder);

        builder.Property(rt => rt.UserId)
               .IsRequired();

        builder.Property(rt => rt.Type)
               .HasConversion<LowerCaseEnumConverter<TokenType>>()
               .IsRequired();

        builder.Property(rt => rt.TokenHash)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(rt => rt.ExpiresAt)
               .IsRequired();

        builder.Property(rt => rt.IsRevoked)
               .IsRequired()
               .HasDefaultValue(false);

        // Manual override remains because it doesn't follow the default snake_case of the property name
        builder.Property(rt => rt.ReplacedById);

        builder.Property(rt => rt.RevokedByIp);

        builder.HasIndex(rt => rt.TokenHash)
               .IsUnique();

        builder.HasIndex(rt => new { rt.UserId, rt.Type });

        builder.HasOne(rt => rt.User)
               .WithMany(u => u.SecurityTokens)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DbConstants.DeleteBehavior);
    }
}