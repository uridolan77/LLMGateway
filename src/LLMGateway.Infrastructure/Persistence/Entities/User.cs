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
    /// API keys
    /// </summary>
    public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
}
