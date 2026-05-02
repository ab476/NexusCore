using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // 1. Set the table name with prefix
        builder.ToTable($"{DbConstants.TablePrefix}user_roles");

        // 2. Composite Primary Key
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // 3. Explicitly name the foreign key columns
        builder.Property(ur => ur.UserId)
               .HasColumnName("user_id")
               .IsRequired();

        builder.Property(ur => ur.RoleId)
               .HasColumnName("role_id")
               .IsRequired();

        // 4. Relationships with explicit snake_case constraint names
        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .HasConstraintName($"fk_{DbConstants.TablePrefix}user_roles_users_user_id")
               .OnDelete(DbConstants.DeleteBehavior);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .HasConstraintName($"fk_{DbConstants.TablePrefix}user_roles_roles_role_id")
               .OnDelete(DbConstants.DeleteBehavior);
    }
}