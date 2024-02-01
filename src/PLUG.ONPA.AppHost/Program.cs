var builder = DistributedApplication.CreateBuilder(args);

builder.AddNpmApp("ui", "../PLUG.ONPA.UI")
    .WithServiceBinding(containerPort: 3000, scheme: "http", env: "PORT")
    .AsDockerfileInManifest();

builder.AddProject<Projects.PLUG_ONPA_Apply_Api>("apply-api")
    .WithLaunchProfile("https");

builder.Build().Run();
