using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

namespace NC.AuthService.Infrastructure.Persistence.Configurations;
public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public new void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder, "roles");

        builder.Property(r => r.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(r => r.Description)
               .HasColumnName("description")
               .HasMaxLength(250);

        builder.HasIndex(r => r.Name)
               .HasDatabaseName("ix_roles_name")
               .IsUnique();
    }
}