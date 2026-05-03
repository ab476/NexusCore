namespace NC.AuthService.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    /// <summary> "auth_" </summary>
    protected string TablePrefix => DbConstants.TablePrefix;
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(r => r.LastVersionId)
               .HasColumnName("last_version_id");

        builder.Property(r => r.VersionId)
               .HasColumnName("version_id")
               .IsConcurrencyToken();
    }

    public void Configure(EntityTypeBuilder<T> builder, string tableName)
    {
        builder.ToTable($"{TablePrefix}{tableName}");
    }
}
