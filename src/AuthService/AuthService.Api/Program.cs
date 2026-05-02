using Microsoft.Extensions.Caching.Hybrid;
using NexusCore.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("cache");
builder.Services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024;
    options.MaximumKeyLength = 1024;
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(2)
    };
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.MapGet("/cache/ping/{key}", async (string key, HybridCache cache, CancellationToken cancellationToken) =>
{
    string cacheKey = $"authservice:ping:{key}";

    string value = await cache.GetOrCreateAsync(
        cacheKey,
        async cancel =>
        {
            await Task.Yield();
            return $"pong:{key}:{DateTime.UtcNow:O}";
        },
        cancellationToken: cancellationToken);

    return Results.Ok(new { Key = cacheKey, Value = value });
});

app.Run();
