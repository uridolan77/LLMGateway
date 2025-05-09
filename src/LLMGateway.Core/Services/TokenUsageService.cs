using LLMGateway.Core.Interfaces;
using LLMGateway.Core.Models.TokenUsage;
using LLMGateway.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LLMGateway.Core.Services;

/// <summary>
/// Service for tracking token usage
/// </summary>
public class TokenUsageService : ITokenUsageService
{
    private readonly ILogger<TokenUsageService> _logger;
    private readonly TokenUsageOptions _options;
    private ConcurrentBag<TokenUsageRecord> _inMemoryRecords = new();

    /// <summary>
    /// Constructor
    /// </summary>
    public TokenUsageService(
        IOptions<TokenUsageOptions> options,
        ILogger<TokenUsageService> logger)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <inheritdoc/>
    public Task TrackUsageAsync(TokenUsageRecord record)
    {
        if (!_options.EnableTokenCounting)
        {
            return Task.CompletedTask;
        }

        _logger.LogDebug("Tracking token usage: {Tokens} tokens for model {ModelId}", record.TotalTokens, record.ModelId);

        // For in-memory storage, just add to the concurrent bag
        if (_options.StorageProvider == "InMemory")
        {
            _inMemoryRecords.Add(record);
            
            // Clean up old records
            CleanupOldRecords();
        }
        else
        {
            // For database storage, this would be implemented in the infrastructure layer
            _logger.LogDebug("Token usage tracking with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TokenUsageRecord>> GetUsageForUserAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (_options.StorageProvider == "InMemory")
        {
            var records = _inMemoryRecords
                .Where(r => r.UserId == userId && r.Timestamp >= startDate && r.Timestamp <= endDate)
                .ToList();
            
            return Task.FromResult<IEnumerable<TokenUsageRecord>>(records);
        }
        
        // For database storage, this would be implemented in the infrastructure layer
        _logger.LogDebug("Token usage retrieval with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        return Task.FromResult<IEnumerable<TokenUsageRecord>>(new List<TokenUsageRecord>());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TokenUsageRecord>> GetUsageForApiKeyAsync(string apiKeyId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (_options.StorageProvider == "InMemory")
        {
            var records = _inMemoryRecords
                .Where(r => r.ApiKeyId == apiKeyId && r.Timestamp >= startDate && r.Timestamp <= endDate)
                .ToList();
            
            return Task.FromResult<IEnumerable<TokenUsageRecord>>(records);
        }
        
        // For database storage, this would be implemented in the infrastructure layer
        _logger.LogDebug("Token usage retrieval with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        return Task.FromResult<IEnumerable<TokenUsageRecord>>(new List<TokenUsageRecord>());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TokenUsageRecord>> GetUsageForModelAsync(string modelId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (_options.StorageProvider == "InMemory")
        {
            var records = _inMemoryRecords
                .Where(r => r.ModelId == modelId && r.Timestamp >= startDate && r.Timestamp <= endDate)
                .ToList();
            
            return Task.FromResult<IEnumerable<TokenUsageRecord>>(records);
        }
        
        // For database storage, this would be implemented in the infrastructure layer
        _logger.LogDebug("Token usage retrieval with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        return Task.FromResult<IEnumerable<TokenUsageRecord>>(new List<TokenUsageRecord>());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TokenUsageRecord>> GetUsageForProviderAsync(string provider, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (_options.StorageProvider == "InMemory")
        {
            var records = _inMemoryRecords
                .Where(r => r.Provider == provider && r.Timestamp >= startDate && r.Timestamp <= endDate)
                .ToList();
            
            return Task.FromResult<IEnumerable<TokenUsageRecord>>(records);
        }
        
        // For database storage, this would be implemented in the infrastructure layer
        _logger.LogDebug("Token usage retrieval with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        return Task.FromResult<IEnumerable<TokenUsageRecord>>(new List<TokenUsageRecord>());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<TokenUsageRecord>> GetTotalUsageAsync(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (_options.StorageProvider == "InMemory")
        {
            var records = _inMemoryRecords
                .Where(r => r.Timestamp >= startDate && r.Timestamp <= endDate)
                .ToList();
            
            return Task.FromResult<IEnumerable<TokenUsageRecord>>(records);
        }
        
        // For database storage, this would be implemented in the infrastructure layer
        _logger.LogDebug("Token usage retrieval with provider {Provider} is handled by the infrastructure layer", _options.StorageProvider);
        return Task.FromResult<IEnumerable<TokenUsageRecord>>(new List<TokenUsageRecord>());
    }

    /// <inheritdoc/>
    public async Task<TokenUsageSummary> GetUsageSummaryAsync(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var records = await GetTotalUsageAsync(startDate, endDate);
        
        var summary = new TokenUsageSummary
        {
            TotalPromptTokens = records.Sum(r => r.PromptTokens),
            TotalCompletionTokens = records.Sum(r => r.CompletionTokens),
            TotalTokens = records.Sum(r => r.TotalTokens),
            TotalEstimatedCostUsd = records.Sum(r => r.EstimatedCostUsd)
        };
        
        // Group by model
        var modelGroups = records.GroupBy(r => r.ModelId);
        foreach (var group in modelGroups)
        {
            var modelId = group.Key;
            var modelRecords = group.ToList();
            var provider = modelRecords.FirstOrDefault()?.Provider ?? "unknown";
            
            summary.UsageByModel[modelId] = new ModelUsage
            {
                ModelId = modelId,
                Provider = provider,
                PromptTokens = modelRecords.Sum(r => r.PromptTokens),
                CompletionTokens = modelRecords.Sum(r => r.CompletionTokens),
                TotalTokens = modelRecords.Sum(r => r.TotalTokens),
                EstimatedCostUsd = modelRecords.Sum(r => r.EstimatedCostUsd)
            };
        }
        
        // Group by provider
        var providerGroups = records.GroupBy(r => r.Provider);
        foreach (var group in providerGroups)
        {
            var provider = group.Key;
            var providerRecords = group.ToList();
            
            summary.UsageByProvider[provider] = new ProviderUsage
            {
                Provider = provider,
                PromptTokens = providerRecords.Sum(r => r.PromptTokens),
                CompletionTokens = providerRecords.Sum(r => r.CompletionTokens),
                TotalTokens = providerRecords.Sum(r => r.TotalTokens),
                EstimatedCostUsd = providerRecords.Sum(r => r.EstimatedCostUsd)
            };
        }
        
        // Group by user
        var userGroups = records.GroupBy(r => r.UserId);
        foreach (var group in userGroups)
        {
            var userId = group.Key;
            var userRecords = group.ToList();
            
            summary.UsageByUser[userId] = new UserUsage
            {
                UserId = userId,
                PromptTokens = userRecords.Sum(r => r.PromptTokens),
                CompletionTokens = userRecords.Sum(r => r.CompletionTokens),
                TotalTokens = userRecords.Sum(r => r.TotalTokens),
                EstimatedCostUsd = userRecords.Sum(r => r.EstimatedCostUsd)
            };
        }
        
        return summary;
    }

    /// <inheritdoc/>
    public Task TrackCompletionTokenUsageAsync(Models.Completion.CompletionRequest request, Models.Completion.CompletionResponse response)
    {
        try
        {
            var record = new TokenUsageRecord
            {
                RequestId = response.Id,
                ModelId = response.Model,
                Provider = response.Provider,
                RequestType = "completion",
                PromptTokens = response.Usage.PromptTokens,
                CompletionTokens = response.Usage.CompletionTokens,
                TotalTokens = response.Usage.TotalTokens,
                UserId = request.User ?? "anonymous",
                ApiKeyId = "unknown" // This would be set by middleware
            };

            // Calculate estimated cost if token prices are available
            // This would be based on the model's token prices

            return TrackUsageAsync(record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to track token usage for completion");
            return Task.CompletedTask;
        }
    }

    private void CleanupOldRecords()
    {
        if (_options.StorageProvider != "InMemory")
        {
            return;
        }
        
        var cutoffDate = DateTimeOffset.UtcNow - _options.DataRetentionPeriod;
        var newRecords = new ConcurrentBag<TokenUsageRecord>();
        
        foreach (var record in _inMemoryRecords)
        {
            if (record.Timestamp >= cutoffDate)
            {
                newRecords.Add(record);
            }
        }
        
        // This is a simplistic approach - in a real implementation, we would use a more efficient
        // data structure or a background job to clean up old records
        Interlocked.Exchange(ref _inMemoryRecords, newRecords);
    }
}
