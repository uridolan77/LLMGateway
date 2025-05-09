using LLMGateway.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LLMGateway.Infrastructure.Persistence;

/// <summary>
/// Database context for LLM Gateway
/// </summary>
public class LLMGatewayDbContext : DbContext
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Database context options</param>
    public LLMGatewayDbContext(DbContextOptions<LLMGatewayDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Token usage records
    /// </summary>
    public DbSet<TokenUsageRecord> TokenUsageRecords { get; set; } = null!;
    
    /// <summary>
    /// Provider health records
    /// </summary>
    public DbSet<ProviderHealthRecord> ProviderHealthRecords { get; set; } = null!;
    
    /// <summary>
    /// Model metrics records
    /// </summary>
    public DbSet<ModelMetricsRecord> ModelMetricsRecords { get; set; } = null!;
    
    /// <summary>
    /// API keys
    /// </summary>
    public DbSet<ApiKey> ApiKeys { get; set; } = null!;
    
    /// <summary>
    /// Users
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure token usage records
        modelBuilder.Entity<TokenUsageRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ApiKeyId).IsRequired();
            entity.Property(e => e.RequestId).IsRequired();
            entity.Property(e => e.ModelId).IsRequired();
            entity.Property(e => e.Provider).IsRequired();
            entity.Property(e => e.RequestType).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.PromptTokens).IsRequired();
            entity.Property(e => e.CompletionTokens).IsRequired();
            entity.Property(e => e.TotalTokens).IsRequired();
            entity.Property(e => e.EstimatedCostUsd).HasPrecision(18, 6).IsRequired();
            
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ApiKeyId);
            entity.HasIndex(e => e.ModelId);
            entity.HasIndex(e => e.Provider);
            entity.HasIndex(e => e.Timestamp);
        });
        
        // Configure provider health records
        modelBuilder.Entity<ProviderHealthRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Provider).IsRequired();
            entity.Property(e => e.IsAvailable).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.ResponseTimeMs).IsRequired();
            
            entity.HasIndex(e => e.Provider);
            entity.HasIndex(e => e.Timestamp);
        });
        
        // Configure model metrics records
        modelBuilder.Entity<ModelMetricsRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ModelId).IsRequired();
            entity.Property(e => e.Provider).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.RequestCount).IsRequired();
            entity.Property(e => e.SuccessCount).IsRequired();
            entity.Property(e => e.FailureCount).IsRequired();
            entity.Property(e => e.TotalTokens).IsRequired();
            entity.Property(e => e.AverageResponseTimeMs).IsRequired();
            entity.Property(e => e.TotalCostUsd).HasPrecision(18, 6).IsRequired();
            
            entity.HasIndex(e => e.ModelId);
            entity.HasIndex(e => e.Provider);
            entity.HasIndex(e => e.Timestamp);
        });
        
        // Configure API keys
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Key).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ExpiresAt);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.Permissions).IsRequired();
            entity.Property(e => e.DailyTokenLimit).IsRequired();
            entity.Property(e => e.MonthlyTokenLimit).IsRequired();
            
            entity.HasIndex(e => e.Key).IsUnique();
            entity.HasIndex(e => e.UserId);
        });
        
        // Configure users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.HasMany(e => e.ApiKeys)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
