using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NC.AuthService.Domain;

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

        builder.Property(r => r.ConcurrencyStamp)
               .HasColumnName("concurrency_stamp")
               .IsConcurrencyToken();
    }

    public void Configure(EntityTypeBuilder<T> builder, string tableName)
    {
        builder.ToTable($"{TablePrefix}{tableName}");
    }
}
