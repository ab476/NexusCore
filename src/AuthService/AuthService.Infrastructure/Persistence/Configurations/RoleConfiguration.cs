namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public new void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(r => r.NormalizedName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(r => r.Description)
               .HasMaxLength(250);

        builder.HasIndex(r => r.NormalizedName)
               .IsUnique();
    }
}