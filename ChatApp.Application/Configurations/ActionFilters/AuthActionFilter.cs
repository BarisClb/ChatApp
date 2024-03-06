using ChatApp.Application.Helpers;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Resources;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text;

namespace ChatApp.Application.Configurations.ActionFilters
{
    public class AuthActionFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly CurrentUser _currentUser;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public AuthActionFilter(CurrentUser currentUser, IStringLocalizer<LocalizationResources> localization)
        {
            _currentUser = currentUser;
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_currentUser?.UserId is null)
                throw new AuthorizationException() { PublicErrorMessage = _localization.GetString(LocalizationKeys.InvalidToken), Method = "HttpRequestHelper.GetCurrentUserWithHeaders", ErrorCode = (int)HttpStatusCode.Unauthorized };
        }
    }
}
