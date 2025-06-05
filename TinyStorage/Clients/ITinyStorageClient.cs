namespace TinyStorage.Clients;

public interface ITinyStorageClient
{
    [Post("/v1/items")]
    Task<ApiResponse<CreateItemResponse>> CreateItemAsync([Body] CreateItemRequest request);
    
    [Delete("/v1/items/{itemId}")]
    Task<IApiResponse> DeleteItemAsync(Guid itemId);

    [Post("/v1/items/{itemId}/is-taken")]
    Task<IApiResponse> TakeGiveItemAsync(Guid itemId, [Body] TakeGiveRequest request);
}