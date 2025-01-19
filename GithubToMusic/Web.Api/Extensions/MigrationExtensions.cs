using GithubCommitsToMusic.Infrastructure;
using GithubCommitsToMusic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GithubCommitsToMusic.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        CancellationTokenSource cts = new();
        if ((await dbContext.Database.GetPendingMigrationsAsync(cts.Token)).Any())
        {
            await dbContext.Database.MigrateAsync(cts.Token);
        }
    }
    public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration) =>
      services
          .AddDatabase(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseSqlServer(connectionString));
        //.UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
