var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NexusCore>("nexuscore");

builder.Build().Run();
