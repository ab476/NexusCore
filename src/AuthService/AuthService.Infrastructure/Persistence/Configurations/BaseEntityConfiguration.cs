namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    /// <summary> "auth_" </summary>
    protected string TablePrefix => DbConstants.TablePrefix;
    public void Configure(EntityTypeBuilder<T> builder)
    {

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id);

        builder.Property(r => r.Version)
               .IsConcurrencyToken();
    }
}
