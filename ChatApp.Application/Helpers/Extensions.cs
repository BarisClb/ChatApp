using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ChatApp.Application.Helpers
{
    public static class Extensions
    {
        public static string? GetHeaderValue(this HttpRequest request, string key)
        {
            return request.Headers.TryGetValue(key, out StringValues keys) ? keys.ToString() : null;
        }
    }
}
