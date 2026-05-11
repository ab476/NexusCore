using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata;
using NC.AuthService.Infrastructure.Persistence.Configurations;

namespace NC.AuthService.Infrastructure.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ToSnakeCaseTable<TEntity>(
        this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        return builder.ToSnakeCaseTable(DbConstants.TablePrefix);
    }
    /// <summary>
    /// Sets the table name to a snake_case version of the entity name with a defined prefix.
    /// Example: User -> auth_users
    /// </summary>
    public static EntityTypeBuilder<TEntity> ToSnakeCaseTable<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        string prefix) where TEntity : class
    {
        // Get the class name, underscore it (snake_case), and pluralize it
        // We use Humanize() then Underscore() to ensure proper formatting
        string tableName = typeof(TEntity).Name.Pluralize().Underscore();

        // Apply the prefix and set the table
        builder.ToTable($"{prefix}{tableName}");

        return builder;
    }

    /// <summary>
    /// Automatically sets the column name to the snake_case version of the property name.
    /// </summary>
    public static PropertyBuilder<TProperty> HasSnakeCaseColumnName<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder)
    {
        // Access the property name from the metadata and underscore it
        var columnName = propertyBuilder.Metadata.Name.Underscore();

        return propertyBuilder.HasColumnName(columnName);
    }

    public static IndexBuilder HasSnakeCaseDatabaseName(
        this IndexBuilder indexBuilder)
    {
        return indexBuilder.HasSnakeCaseDatabaseName(DbConstants.TablePrefix);
    }
    /// <summary>
    /// Automatically sets the index name using the pattern: ix_prefix_tablename_columnname
    /// </summary>
    public static IndexBuilder HasSnakeCaseDatabaseName(
        this IndexBuilder indexBuilder,
        string prefix)
    {
        // 1. Get the table name
        var tableName = indexBuilder.Metadata.DeclaringEntityType.GetTableName();

        // 2. Get all column names associated with this index (handles composite indexes)
        var columnNames = string.Join("_", indexBuilder.Metadata.Properties
            .Select(p => p.GetColumnName(StoreObjectIdentifier.Table(tableName!, null))));

        // 3. Construct the name: ix + prefix + table + columns
        // We use Underscore() to ensure the final result is clean snake_case
        var indexName = $"ix_{prefix}{tableName}_{columnNames}".Underscore();

        return indexBuilder.HasDatabaseName(indexName);
    }
}
