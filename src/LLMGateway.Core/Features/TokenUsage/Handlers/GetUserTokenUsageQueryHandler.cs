using LLMGateway.Core.CQRS;
using LLMGateway.Core.Features.TokenUsage.Queries;
using LLMGateway.Core.Models.TokenUsage;
using LLMGateway.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace LLMGateway.Core.Features.TokenUsage.Handlers;

/// <summary>
/// Handler for GetUserTokenUsageQuery
/// </summary>
public class GetUserTokenUsageQueryHandler : IQueryHandler<GetUserTokenUsageQuery, IEnumerable<TokenUsageRecord>>
{
    private readonly ITokenUsageRepository _tokenUsageRepository;
    private readonly ILogger<GetUserTokenUsageQueryHandler> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tokenUsageRepository">Token usage repository</param>
    /// <param name="logger">Logger</param>
    public GetUserTokenUsageQueryHandler(
        ITokenUsageRepository tokenUsageRepository,
        ILogger<GetUserTokenUsageQueryHandler> logger)
    {
        _tokenUsageRepository = tokenUsageRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handle the query
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token usage records for the user</returns>
    public async Task<IEnumerable<TokenUsageRecord>> Handle(GetUserTokenUsageQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUserTokenUsageQuery for user: {UserId}", request.UserId);
        
        var records = await _tokenUsageRepository.GetByUserIdAsync(
            request.UserId, 
            request.StartDate, 
            request.EndDate);
        
        return records.Select(MapEntityToModel);
    }
    
    private static TokenUsageRecord MapEntityToModel(Infrastructure.Persistence.Entities.TokenUsageRecord entity)
    {
        return new TokenUsageRecord
        {
            Id = entity.Id,
            Timestamp = entity.Timestamp,
            UserId = entity.UserId,
            ApiKeyId = entity.ApiKeyId,
            RequestId = entity.RequestId,
            ModelId = entity.ModelId,
            Provider = entity.Provider,
            RequestType = entity.RequestType,
            PromptTokens = entity.PromptTokens,
            CompletionTokens = entity.CompletionTokens,
            TotalTokens = entity.TotalTokens,
            Cost = entity.Cost
        };
    }
}
