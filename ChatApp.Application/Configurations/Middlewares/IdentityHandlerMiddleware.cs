using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.Configurations.Middlewares
{
    public class IdentityHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpRequestHelper _httpRequestHelper;

        public IdentityHandlerMiddleware(RequestDelegate next, IHttpRequestHelper httpRequestHelper)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _httpRequestHelper = httpRequestHelper ?? throw new ArgumentNullException(nameof(httpRequestHelper));
        }


        public async Task Invoke(HttpContext context, CurrentUser currentUser)
        {
            await SetIdentityForRequest(currentUser);
            await _next(context);
        }

        private async Task SetIdentityForRequest(CurrentUser currentUser)
        {
            //var requestIdentity = await _httpRequestHelper.GetCurrentUser();
            //currentUser.UserId = 1;
            //currentUser.UserStatus = null;
            //currentUser.FirstName = "baris";
            //currentUser.LastName = "celebi";
            //currentUser.EmailAddress = "barisclb1903@gmail.com";
            //currentUser.Username = requestIdentity.Username;
            //currentUser.LanguageCode = requestIdentity.LanguageCode;
            //currentUser.UserDateCreated = requestIdentity.UserDateCreated;
            //currentUser.IsAdmin = requestIdentity.IsAdmin;
            //currentUser.RefreshTokenId = requestIdentity.RefreshTokenId;
            var requestIdentity = await _httpRequestHelper.GetCurrentUser();
            currentUser.UserId = requestIdentity.UserId;
            currentUser.UserStatus = requestIdentity.UserStatus;
            currentUser.FirstName = requestIdentity.FirstName;
            currentUser.LastName = requestIdentity.LastName;
            currentUser.EmailAddress = requestIdentity.EmailAddress;
            currentUser.Username = requestIdentity.Username;
            currentUser.LanguageCode = requestIdentity.LanguageCode;
            currentUser.UserDateCreated = requestIdentity.UserDateCreated;
            currentUser.IsAdmin = requestIdentity.IsAdmin;
        }
    }
}
