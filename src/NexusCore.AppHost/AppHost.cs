var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();
var cache = builder.AddRedis("cache");

builder.AddProject<Projects.NexusCore>("nexuscore");

builder.AddProject<Projects.AuthService_Api>("authservice-api")
    .WithReference(cache);

builder.Build().Run();
