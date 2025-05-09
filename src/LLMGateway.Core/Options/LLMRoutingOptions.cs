using LLMGateway.Core.Models.Routing;

namespace LLMGateway.Core.Options;

/// <summary>
/// Options for LLM routing
/// </summary>
public class LLMRoutingOptions
{
    /// <summary>
    /// Whether to use dynamic routing
    /// </summary>
    public bool UseDynamicRouting { get; set; } = true;
    
    /// <summary>
    /// Model mappings for routing
    /// </summary>
    public List<ModelMapping> ModelMappings { get; set; } = new();
}

/// <summary>
/// Options for routing strategies
/// </summary>
public class RoutingOptions
{
    /// <summary>
    /// Whether to enable smart routing
    /// </summary>
    public bool EnableSmartRouting { get; set; } = true;
    
    /// <summary>
    /// Whether to enable load balancing
    /// </summary>
    public bool EnableLoadBalancing { get; set; } = true;
    
    /// <summary>
    /// Whether to enable latency-optimized routing
    /// </summary>
    public bool EnableLatencyOptimizedRouting { get; set; } = true;
    
    /// <summary>
    /// Whether to enable cost-optimized routing
    /// </summary>
    public bool EnableCostOptimizedRouting { get; set; } = true;
    
    /// <summary>
    /// Whether to enable content-based routing
    /// </summary>
    public bool EnableContentBasedRouting { get; set; } = true;
    
    /// <summary>
    /// Whether to enable quality-optimized routing
    /// </summary>
    public bool EnableQualityOptimizedRouting { get; set; } = true;
    
    /// <summary>
    /// Whether to track routing decisions
    /// </summary>
    public bool TrackRoutingDecisions { get; set; } = true;
    
    /// <summary>
    /// Whether to track model metrics
    /// </summary>
    public bool TrackModelMetrics { get; set; } = true;
    
    /// <summary>
    /// Whether to enable experimental routing
    /// </summary>
    public bool EnableExperimentalRouting { get; set; } = false;
    
    /// <summary>
    /// Sampling rate for experimental routing
    /// </summary>
    public double ExperimentalSamplingRate { get; set; } = 0.1;
    
    /// <summary>
    /// Experimental models to include in routing
    /// </summary>
    public List<string> ExperimentalModels { get; set; } = new();
    
    /// <summary>
    /// Model mappings for routing
    /// </summary>
    public List<ModelRouteMapping> ModelMappings { get; set; } = new();
    
    /// <summary>
    /// Model routing strategies
    /// </summary>
    public List<ModelRoutingStrategy> ModelRoutingStrategies { get; set; } = new();
}

/// <summary>
/// Model route mapping
/// </summary>
public class ModelRouteMapping
{
    /// <summary>
    /// Source model ID
    /// </summary>
    public string ModelId { get; set; } = string.Empty;
    
    /// <summary>
    /// Target model ID
    /// </summary>
    public string TargetModelId { get; set; } = string.Empty;
}

/// <summary>
/// Model routing strategy
/// </summary>
public class ModelRoutingStrategy
{
    /// <summary>
    /// Model ID
    /// </summary>
    public string ModelId { get; set; } = string.Empty;
    
    /// <summary>
    /// Routing strategy
    /// </summary>
    public string Strategy { get; set; } = string.Empty;
}

/// <summary>
/// User preferences for routing
/// </summary>
public class UserPreferencesOptions
{
    /// <summary>
    /// User routing preferences
    /// </summary>
    public List<UserRoutingPreference> UserRoutingPreferences { get; set; } = new();
    
    /// <summary>
    /// User model preferences
    /// </summary>
    public List<UserModelPreference> UserModelPreferences { get; set; } = new();
}

/// <summary>
/// User routing preference
/// </summary>
public class UserRoutingPreference
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Routing strategy
    /// </summary>
    public string RoutingStrategy { get; set; } = string.Empty;
}

/// <summary>
/// User model preference
/// </summary>
public class UserModelPreference
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Preferred model ID
    /// </summary>
    public string PreferredModelId { get; set; } = string.Empty;
}
