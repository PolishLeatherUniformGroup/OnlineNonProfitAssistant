var builder = DistributedApplication.CreateBuilder(args);

builder.AddNpmApp("ui", "../PLUG.ONPA.UI")
    .WithServiceBinding(containerPort: 3000, scheme: "http", env: "PORT")
    .AsDockerfileInManifest();

builder.Build().Run();
