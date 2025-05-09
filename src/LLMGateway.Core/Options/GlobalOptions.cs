namespace LLMGateway.Core.Options;

/// <summary>
/// Global options for the LLM Gateway
/// </summary>
public class GlobalOptions
{
    /// <summary>
    /// Whether to enable caching
    /// </summary>
    public bool EnableCaching { get; set; } = true;
    
    /// <summary>
    /// Cache expiration time in minutes
    /// </summary>
    public int CacheExpirationMinutes { get; set; } = 60;
    
    /// <summary>
    /// Whether to track token usage
    /// </summary>
    public bool TrackTokenUsage { get; set; } = true;
    
    /// <summary>
    /// Whether to enable provider discovery
    /// </summary>
    public bool EnableProviderDiscovery { get; set; } = true;
    
    /// <summary>
    /// Default timeout for requests in seconds
    /// </summary>
    public int DefaultTimeoutSeconds { get; set; } = 30;
    
    /// <summary>
    /// Default timeout for streaming requests in seconds
    /// </summary>
    public int DefaultStreamTimeoutSeconds { get; set; } = 120;
}
