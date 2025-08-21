using GithubCommitsToMusic.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GithubCommitsToMusic.Infrastructure;

public static class InitialiserExtensions
{
    public static void AddAsyncSeeding(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
    {
        builder.UseAsyncSeeding(async (context, _, ct) =>
        {
            var initialiser = serviceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.SeedAsync();
        });

        builder.UseSeeding((context, _) =>
        {
            var initialiser = serviceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            // Block on async call for sync context
            initialiser.SeedAsync().GetAwaiter().GetResult();
        });
    }

    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, IHostEnvironment env)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            var hasPendingChanges = _context.Database.HasPendingModelChanges();
            var hasPendingMigrations = (await _context.Database.GetPendingMigrationsAsync()).Any();
            
            if (hasPendingChanges
                || !hasPendingMigrations)
                return;
            _logger.LogInformation("Initialising the database...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Initialising database finished successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        //var administratorRole = new IdentityRole(Roles.Administrator);

        //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        //{
        //    await _roleManager.CreateAsync(administratorRole);
        //}

        //// Default users
        //var administrator = new User
        //{
        //    UserName = "administrator@localhost",
        //    Email = "administrator@localhost",
        //    FirstName = "Yunus",
        //    LastName = "Özdemir",
        //    BirthDate = DateTime.Now,
        //    Gender = Domain.Enums.Gender.Male
        //};

        //if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        //{
        //    await _userManager.CreateAsync(administrator, "Administrator1!");
        //    if (!string.IsNullOrWhiteSpace(administratorRole.Name))
        //    {
        //        await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        //    }
        //}
        //await _context.SaveChangesAsync();
    }
}
