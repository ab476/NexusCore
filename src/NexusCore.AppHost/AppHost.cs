var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

builder.AddProject<Projects.NexusCore>("nexuscore");

builder.AddProject<Projects.AuthService_Api>("authservice-api");

builder.Build().Run();
