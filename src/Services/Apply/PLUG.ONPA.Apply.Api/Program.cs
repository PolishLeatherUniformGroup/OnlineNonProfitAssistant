using System.Reflection;
using PLUG.ONPA.Apply.Api.Repositories;
using PLUG.ONPA.Apply.Api.Services;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddTransient<IAggregateRepository<ApplicationAggregate>, ApplicationAggregateRepository>();
builder.Services.AddTransient<ITenantSettingsService, TenantSettingsService>();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStatusCodePages();
app.UseExceptionHandler();

app.Run();
