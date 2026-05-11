namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public new void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(512);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(u => u.Email)
               .IsUnique();
    }
}