using FreeCourse.Gateway.DelegateHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json");

builder.Services.AddHttpClient<TokenExhangeDelegateHandler>();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});
// paymentservice ve discountservice istek yap�l�nca cal��acak olan token exchange handler
builder.Services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();

var app = builder.Build();

await app.UseOcelot();

await app.RunAsync();
