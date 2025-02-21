using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json")
    .AddEnvironmentVariables();

var deneme = builder.Environment.EnvironmentName.ToLower();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

await app.RunAsync();
