var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ELK_App>("elk-app");

builder.Build().Run();
