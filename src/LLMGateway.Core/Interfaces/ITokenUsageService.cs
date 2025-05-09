using LLMGateway.Core.Models.TokenUsage;

namespace LLMGateway.Core.Interfaces;

/// <summary>
/// Service for tracking token usage
/// </summary>
public interface ITokenUsageService
{
    /// <summary>
    /// Track token usage
    /// </summary>
    /// <param name="record">Token usage record</param>
    /// <returns>Task</returns>
    Task TrackUsageAsync(TokenUsageRecord record);
    
    /// <summary>
    /// Track token usage for a completion request and response
    /// </summary>
    /// <param name="request">Completion request</param>
    /// <param name="response">Completion response</param>
    /// <returns>Task</returns>
    Task TrackCompletionTokenUsageAsync(Models.Completion.CompletionRequest request, Models.Completion.CompletionResponse response);
    
    /// <summary>
    /// Get token usage for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of token usage records</returns>
    Task<IEnumerable<TokenUsageRecord>> GetUsageForUserAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate);
    
    /// <summary>
    /// Get token usage for an API key
    /// </summary>
    /// <param name="apiKeyId">API key ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of token usage records</returns>
    Task<IEnumerable<TokenUsageRecord>> GetUsageForApiKeyAsync(string apiKeyId, DateTimeOffset startDate, DateTimeOffset endDate);
    
    /// <summary>
    /// Get token usage for a model
    /// </summary>
    /// <param name="modelId">Model ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of token usage records</returns>
    Task<IEnumerable<TokenUsageRecord>> GetUsageForModelAsync(string modelId, DateTimeOffset startDate, DateTimeOffset endDate);
    
    /// <summary>
    /// Get token usage for a provider
    /// </summary>
    /// <param name="provider">Provider name</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of token usage records</returns>
    Task<IEnumerable<TokenUsageRecord>> GetUsageForProviderAsync(string provider, DateTimeOffset startDate, DateTimeOffset endDate);
    
    /// <summary>
    /// Get total token usage
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of token usage records</returns>
    Task<IEnumerable<TokenUsageRecord>> GetTotalUsageAsync(DateTimeOffset startDate, DateTimeOffset endDate);
    
    /// <summary>
    /// Get token usage summary
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Token usage summary</returns>
    Task<TokenUsageSummary> GetUsageSummaryAsync(DateTimeOffset startDate, DateTimeOffset endDate);
}

/// <summary>
/// Summary of token usage
/// </summary>
public class TokenUsageSummary
{
    /// <summary>
    /// Total prompt tokens
    /// </summary>
    public int TotalPromptTokens { get; set; }
    
    /// <summary>
    /// Total completion tokens
    /// </summary>
    public int TotalCompletionTokens { get; set; }
    
    /// <summary>
    /// Total tokens
    /// </summary>
    public int TotalTokens { get; set; }
    
    /// <summary>
    /// Total estimated cost in USD
    /// </summary>
    public decimal TotalEstimatedCostUsd { get; set; }
    
    /// <summary>
    /// Usage by model
    /// </summary>
    public Dictionary<string, ModelUsage> UsageByModel { get; set; } = new();
    
    /// <summary>
    /// Usage by provider
    /// </summary>
    public Dictionary<string, ProviderUsage> UsageByProvider { get; set; } = new();
    
    /// <summary>
    /// Usage by user
    /// </summary>
    public Dictionary<string, UserUsage> UsageByUser { get; set; } = new();
}

/// <summary>
/// Usage for a model
/// </summary>
public class ModelUsage
{
    /// <summary>
    /// Model ID
    /// </summary>
    public string ModelId { get; set; } = string.Empty;
    
    /// <summary>
    /// Provider name
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Prompt tokens
    /// </summary>
    public int PromptTokens { get; set; }
    
    /// <summary>
    /// Completion tokens
    /// </summary>
    public int CompletionTokens { get; set; }
    
    /// <summary>
    /// Total tokens
    /// </summary>
    public int TotalTokens { get; set; }
    
    /// <summary>
    /// Estimated cost in USD
    /// </summary>
    public decimal EstimatedCostUsd { get; set; }
}

/// <summary>
/// Usage for a provider
/// </summary>
public class ProviderUsage
{
    /// <summary>
    /// Provider name
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Prompt tokens
    /// </summary>
    public int PromptTokens { get; set; }
    
    /// <summary>
    /// Completion tokens
    /// </summary>
    public int CompletionTokens { get; set; }
    
    /// <summary>
    /// Total tokens
    /// </summary>
    public int TotalTokens { get; set; }
    
    /// <summary>
    /// Estimated cost in USD
    /// </summary>
    public decimal EstimatedCostUsd { get; set; }
}

/// <summary>
/// Usage for a user
/// </summary>
public class UserUsage
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Prompt tokens
    /// </summary>
    public int PromptTokens { get; set; }
    
    /// <summary>
    /// Completion tokens
    /// </summary>
    public int CompletionTokens { get; set; }
    
    /// <summary>
    /// Total tokens
    /// </summary>
    public int TotalTokens { get; set; }
    
    /// <summary>
    /// Estimated cost in USD
    /// </summary>
    public decimal EstimatedCostUsd { get; set; }
}
