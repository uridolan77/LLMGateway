using LLMGateway.Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Sqlite;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace LLMGateway.Infrastructure.Persistence.Extensions;

/// <summary>
/// Extensions for persistence
/// </summary>
public static class PersistenceExtensions
{
    /// <summary>
    /// Add persistence
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var persistenceOptions = new PersistenceOptions();
        configuration.GetSection("Persistence").Bind(persistenceOptions);
        
        if (!persistenceOptions.UseDatabase)
        {
            return services;
        }
        
        // Add the database context
        services.AddDbContext<LLMGatewayDbContext>(options =>
        {
            switch (persistenceOptions.DatabaseProvider.ToLowerInvariant())
            {
                case "sqlserver":
                    options.UseSqlServer(persistenceOptions.ConnectionString);
                    break;
                    
                case "postgresql":
                    // Use dynamic approach to avoid direct reference
                    var npgsqlMethod = typeof(DbContextOptionsBuilder)
                        .GetMethod("UseNpgsql", new[] { typeof(string) });
                    
                    if (npgsqlMethod != null)
                    {
                        npgsqlMethod.Invoke(options, new object[] { persistenceOptions.ConnectionString });
                    }
                    else
                    {
                        throw new ArgumentException("Npgsql provider not available. Please install the Npgsql.EntityFrameworkCore.PostgreSQL package.");
                    }
                    break;
                    
                case "sqlite":
                    // Use dynamic approach to avoid direct reference
                    var sqliteMethod = typeof(DbContextOptionsBuilder)
                        .GetMethod("UseSqlite", new[] { typeof(string) });
                    
                    if (sqliteMethod != null)
                    {
                        sqliteMethod.Invoke(options, new object[] { persistenceOptions.ConnectionString });
                    }
                    else
                    {
                        throw new ArgumentException("SQLite provider not available. Please install the Microsoft.EntityFrameworkCore.Sqlite package.");
                    }
                    break;
                    
                default:
                    throw new ArgumentException($"Unsupported database provider: {persistenceOptions.DatabaseProvider}");
            }
        });
        
        // Add repositories
        services.AddScoped<ITokenUsageRepository, TokenUsageRepository>();
        
        return services;
    }
    
    /// <summary>
    /// Migrate the database
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <returns>Application builder</returns>
    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LLMGatewayDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LLMGatewayDbContext>>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<PersistenceOptions>>().Value;
        
        if (!options.UseDatabase || !options.EnableMigrations)
        {
            return app;
        }
        
        try
        {
            logger.LogInformation("Applying database migrations");
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations");
            throw;
        }
        
        return app;
    }
}
