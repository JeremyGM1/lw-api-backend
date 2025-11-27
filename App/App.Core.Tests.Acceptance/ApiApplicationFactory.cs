using Identity.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace App.Core.Tests.Acceptance;
internal class ApiApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=db_App_Intership;Trusted_Connection=True;MultipleActiveResultSets=true";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d => d.ServiceType.FullName?.Contains("DbContextOptions") == true
                    || d.ServiceType.FullName?.Contains("AppDbContext") == true
                    || d.ServiceType.FullName?.Contains("TalentSuiteIdentityDbContext") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    TestConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                ),
                ServiceLifetime.Transient
            );

            services.AddDbContext<TalentSuiteIdentityDbContext>(options =>
                options.UseSqlServer(
                    TestConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                ),
                ServiceLifetime.Transient
            );
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                using (var scope = Services.CreateScope())
                {
                    var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    appContext.Database.EnsureDeleted();

                    var identityContext = scope.ServiceProvider.GetRequiredService<TalentSuiteIdentityDbContext>();
                    identityContext.Database.EnsureDeleted();
                }
            }
            catch { }
        }
        base.Dispose(disposing);
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ApiApplicationFactory>()
            .Build();

        var conString = configuration.GetConnectionString("App");
        return conString;
    }

    private static AppDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext;
    }
}