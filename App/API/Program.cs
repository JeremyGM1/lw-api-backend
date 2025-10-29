using API.Extensions;
using Identity;
using Serilog;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));


builder.Services.AddControllers();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AppServiceExtensions();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddRateLimitingPolicies();

builder.Services.AddCorsPolicies();
// **Swagger with JWT**
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseMiddleware<ValidationLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intership_backend v1"));
    app.UseReDoc(opt => opt.SpecUrl("/swagger/v1/swagger.json"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.SeedIdentityAsync();

app.Run();