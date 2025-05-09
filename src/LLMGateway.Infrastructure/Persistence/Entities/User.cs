namespace LLMGateway.Infrastructure.Persistence.Entities;

/// <summary>
/// User entity
/// </summary>
public class User
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Password hash
    /// </summary>
    public string? PasswordHash { get; set; }
    
    /// <summary>
    /// Created at
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Role
    /// </summary>
    public string Role { get; set; } = "User";
    
    /// <summary>
    /// Last login timestamp
    /// </summary>
    public DateTimeOffset? LastLoginAt { get; set; }
    
    /// <summary>
    /// API keys
    /// </summary>
    public virtual ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
    
    /// <summary>
    /// Token usage records
    /// </summary>
    public virtual ICollection<TokenUsageRecord> TokenUsageRecords { get; set; } = new List<TokenUsageRecord>();
    
    /// <summary>
    /// Routing decisions
    /// </summary>
    public virtual ICollection<RoutingDecision> RoutingDecisions { get; set; } = new List<RoutingDecision>();
    
    /// <summary>
    /// User permissions
    /// </summary>
    public virtual ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();
}
