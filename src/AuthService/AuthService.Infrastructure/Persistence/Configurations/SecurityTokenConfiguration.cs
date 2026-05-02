using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class SecurityTokenConfiguration : BaseEntityConfiguration<SecurityToken>
{
    public new void Configure(EntityTypeBuilder<SecurityToken> builder)
    {
        base.Configure(builder, "security_tokens");

        builder.Property(rt => rt.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(rt => rt.Type)
               .HasColumnName("type")
               .HasConversion<int>()
               .IsRequired();

        builder.Property(rt => rt.TokenHash)
               .HasColumnName("token_hash")
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(rt => rt.ExpiresAt)
               .HasColumnName("expires_at")
               .IsRequired();

        builder.Property(rt => rt.IsRevoked)
               .HasColumnName("is_revoked")
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(rt => rt.ReplacedById)
               .HasColumnName("replaced_by_token_id");

        builder.Property(rt => rt.RevokedByIp)
               .HasColumnName("revoked_by_ip");

        // 4. Indexes
        builder.HasIndex(rt => rt.TokenHash)
               .HasDatabaseName($"ix_{DbConstants.TablePrefix}security_tokens_token_hash")
               .IsUnique();

        // Highly recommended for queries like: _context.SecurityTokens.Where(t => t.UserId == id && t.Type == TokenType.RefreshToken)
        builder.HasIndex(rt => new { rt.UserId, rt.Type })
               .HasDatabaseName($"ix_{DbConstants.TablePrefix}security_tokens_user_id_type");

        // 5. Relationships
        builder.HasOne(rt => rt.User)
               .WithMany(u => u.SecurityTokens)
               .HasForeignKey(rt => rt.UserId)
               .HasConstraintName($"fk_{DbConstants.TablePrefix}security_tokens_users_user_id") // Snake case the FK constraint
               .OnDelete(DbConstants.DeleteBehavior);
    }
}