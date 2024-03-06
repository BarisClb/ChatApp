using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Responses.Common;
using MediatR;

namespace ChatApp.Application.Configurations.Pipelines
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableQuery
    {
        private readonly ICacheService _cacheService;

        public CachingBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var key = request.CacheKey;
            if (string.IsNullOrEmpty(key))
                return await next();

            var cachedValue = await _cacheService.GetValueAsync<TResponse>(key);
            if (cachedValue != null)
                return cachedValue;

            var response = await next();

            if (response != null)
            {
                // Check if the response is ApiResponse<T>, if true:
                if (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition().Equals(typeof(ApiResponse<>)))
                {
                    // cache if its a succcess
                    var apiResponseSuccess = (bool)((response?.GetType().GetProperty("IsSuccess")?.GetValue(response)) ?? false);
                    if (apiResponseSuccess)
                        await _cacheService.SetValueAsync(request.CacheKey, response, request.ExpirationTime);

                    // cache if it contains data
                    //var apiResponseData = response?.GetType().GetProperty("Data")?.GetValue(response);
                    //if (apiResponseData != null)
                    //    await _cacheService.SetValueAsync(request.CacheKey, apiResponseData, request.ExpirationTime);
                }
                else
                    await _cacheService.SetValueAsync(request.CacheKey, response, request.ExpirationTime);
            }

            return response;
        }
    }
}
