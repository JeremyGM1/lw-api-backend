using API.Extensions;
using API.Middleware;
using Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AppServiceExtensions();
builder.Services.AddOpenApi();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddRateLimitingPolicies();

builder.Services.AddSwaggerWithJwt();
builder.Services.AddCorsPolicies(builder.Configuration);
// **Swagger with JWT**
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intership_backend v1"));
    app.UseReDoc(opt => opt.SpecUrl("/swagger/v1/swagger.json"));
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.SeedIdentityAsync();

app.Run();