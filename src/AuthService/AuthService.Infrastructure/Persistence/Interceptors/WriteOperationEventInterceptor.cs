//using System.Collections.Concurrent;
//using System.Globalization;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using NC.AuthService.Infrastructure.Persistence.Messaging;
//using NC.Messaging;

//namespace NC.AuthService.Infrastructure.Persistence.Interceptors;

//public sealed class WriteOperationEventInterceptor(IMessageBus messageBus) : SaveChangesInterceptor
//{
//    private const string TopicName = "entity.write";
//    private readonly ConcurrentDictionary<string, EntityWriteEvent[]> _pendingEvents = new();

//    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
//    {
//        CapturePendingEvents(eventData.Context);
//        return base.SavingChanges(eventData, result);
//    }

//    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
//        DbContextEventData eventData,
//        InterceptionResult<int> result,
//        CancellationToken cancellationToken = default)
//    {
//        CapturePendingEvents(eventData.Context);
//        return base.SavingChangesAsync(eventData, result, cancellationToken);
//    }

//    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
//    {
//        PublishPendingEventsAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
//        return base.SavedChanges(eventData, result);
//    }

//    public override async ValueTask<int> SavedChangesAsync(
//        SaveChangesCompletedEventData eventData,
//        int result,
//        CancellationToken cancellationToken = default)
//    {
//        await PublishPendingEventsAsync(eventData.Context, cancellationToken);
//        return await base.SavedChangesAsync(eventData, result, cancellationToken);
//    }

//    public override void SaveChangesFailed(DbContextErrorEventData eventData)
//    {
//        DiscardPendingEvents(eventData.Context);
//        base.SaveChangesFailed(eventData);
//    }

//    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
//    {
//        DiscardPendingEvents(eventData.Context);
//        return base.SaveChangesFailedAsync(eventData, cancellationToken);
//    }

//    private void CapturePendingEvents(DbContext? context)
//    {
//        if (context == null)
//        {
//            return;
//        }

//        context.ChangeTracker.DetectChanges();

//        EntityWriteEvent[] writeEvents = context.ChangeTracker
//            .Entries()
//            .Where(IsWriteOperation)
//            .Select(CreateWriteEvent)
//            .ToArray();

//        string contextId = context.ContextId.ToString();
//        if (writeEvents.Length == 0)
//        {
//            _pendingEvents.TryRemove(contextId, out _);
//            return;
//        }

//        _pendingEvents[contextId] = writeEvents;
//    }

//    private async Task PublishPendingEventsAsync(DbContext? context, CancellationToken cancellationToken)
//    {
//        if (context == null)
//        {
//            return;
//        }

//        if (!_pendingEvents.TryRemove(context.ContextId.ToString(), out EntityWriteEvent[]? writeEvents))
//        {
//            return;
//        }

//        foreach (EntityWriteEvent writeEvent in writeEvents)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            await messageBus.PublishToTopicAsync(TopicName, writeEvent);
//        }
//    }

//    private void DiscardPendingEvents(DbContext? context)
//    {
//        if (context is null)
//        {
//            return;
//        }

//        _pendingEvents.TryRemove(context.ContextId.ToString(), out _);
//    }

//    internal static bool IsWriteOperation(EntityEntry entry)
//    {
//        return entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted;
//    }

//    private static EntityWriteEvent CreateWriteEvent(EntityEntry entry)
//    {
//        return new EntityWriteEvent
//        {
//            EntityName = entry.Metadata.ClrType.Name,
//            Operation = MapOperation(entry.State),
//            OccurredAtUtc = DateTime.UtcNow,
//            Keys = entry.Properties
//                .Where(property => property.Metadata.IsPrimaryKey())
//                .Select(property => new EntityKeyValue
//                {
//                    Name = property.Metadata.Name,
//                    Value = FormatValue(GetPropertyValue(entry.State == EntityState.Deleted, property))
//                })
//                .ToArray(),
//            Changes = GetChanges(entry)
//        };
//    }

//    private static EntityPropertyChange[] GetChanges(EntityEntry entry)
//    {
//        IEnumerable<PropertyEntry> properties = entry.State switch
//        {
//            EntityState.Added => entry.Properties.Where(property => !property.Metadata.IsPrimaryKey()),
//            EntityState.Modified => entry.Properties.Where(property => property.IsModified && !property.Metadata.IsPrimaryKey()),
//            EntityState.Deleted => entry.Properties.Where(property => !property.Metadata.IsPrimaryKey()),
//            _ => []
//        };

//        return properties
//            .Select(property => new EntityPropertyChange
//            {
//                Name = property.Metadata.Name,
//                OriginalValue = entry.State == EntityState.Added ? null : FormatValue(property.OriginalValue),
//                CurrentValue = entry.State == EntityState.Deleted ? null : FormatValue(property.CurrentValue)
//            })
//            .ToArray();
//    }

//    private static object? GetPropertyValue(bool useOriginalValue, PropertyEntry property)
//        => useOriginalValue ? property.OriginalValue : property.CurrentValue;

//    private static string MapOperation(EntityState state) => state switch
//    {
//        EntityState.Added => "created",
//        EntityState.Modified => "updated",
//        EntityState.Deleted => "deleted",
//        _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Unsupported entity state.")
//    };

//    private static string? FormatValue(object? value) => value switch
//    {
//        null => null,
//        DateTime dateTime => dateTime.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
//        DateTimeOffset dateTimeOffset => dateTimeOffset.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
//        byte[] bytes => Convert.ToBase64String(bytes),
//        IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
//        _ => value.ToString()
//    };
//}
