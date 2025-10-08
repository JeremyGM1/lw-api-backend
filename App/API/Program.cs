using API.extensions;
using API.Extentions;
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

app.Run();