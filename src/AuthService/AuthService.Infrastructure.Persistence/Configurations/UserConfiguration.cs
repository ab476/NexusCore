using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;
using System;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public new void Configure(EntityTypeBuilder<User> builder)
    {
        // 1. Let the base class handle Id, CreatedAt, and ConcurrencyStamp
        base.Configure(builder);

        // 2. Set the table name with prefix
        builder.ToTable($"{DbConstants.TablePrefix}users");

        // 3. Configure User specific properties
        builder.Property(u => u.Email)
               .HasColumnName("email")
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired()
               .HasMaxLength(512);

        builder.Property(u => u.FirstName)
               .HasColumnName("first_name")
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .HasColumnName("last_name")
               .IsRequired()
               .HasMaxLength(100);

        // 4. Indexes
        builder.HasIndex(u => u.Email)
               .HasDatabaseName($"ix_{DbConstants.TablePrefix}users_email")
               .IsUnique();
    }
}