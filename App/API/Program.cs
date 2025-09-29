using API.Extentions;
using API.Middleware;
using Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AppServiceExtensions(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddRateLimitingPolicies();

// **Swagger with JWT**
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intership_backend v1"));
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();