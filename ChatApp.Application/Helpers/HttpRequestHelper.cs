using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Resources;
using ChatApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ChatApp.Application.Helpers
{
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public HttpRequestHelper(IHttpContextAccessor httpContextAccessor, ITokenService tokenService, ICacheService cacheService, IStringLocalizer<LocalizationResources> localization)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public async Task<CurrentUser> GetCurrentUser()
        {
            var headerValues = await GetHeaders();

            if (string.IsNullOrEmpty(headerValues?.AccessToken) || (!headerValues?.AccessToken?.StartsWith("Bearer") ?? true))
                return new();

            AccessTokenClaimsData? accessTokenClaims = await _tokenService.GetAccessTokenClaims(headerValues.AccessToken);

            if (accessTokenClaims?.UserId != null)
            {
                var accessTokenId = await _cacheService.GetValueAsync<Guid?>(accessTokenClaims.UserId);
                if (accessTokenId == default || !(Guid.TryParse(accessTokenClaims.AccessTokenId, out Guid parsedAccessTokenId)) || accessTokenId != parsedAccessTokenId)
                    return new();
            }

            return new()
            {
                UserId = (accessTokenClaims?.UserId != null && Guid.TryParse(accessTokenClaims.UserId, out Guid userIdParseResult)) ? userIdParseResult : null,
                FirstName = accessTokenClaims?.FirstName,
                LastName = accessTokenClaims?.LastName,
                Username = accessTokenClaims?.Username,
                EmailAddress = accessTokenClaims?.EmailAddress,
                UserStatus = (accessTokenClaims?.UserStatus != null && Int32.TryParse(accessTokenClaims.UserId, out int userStatusParseResult)) ? ((UserStatusType)userStatusParseResult) : null,
                LanguageCode = accessTokenClaims?.LanguageCode,
                UserDateCreated = (accessTokenClaims?.UserDateCreated != null && DateTime.TryParse(accessTokenClaims.UserId, CultureInfo.InvariantCulture, out DateTime userDateCreatedParseResult)) ? userDateCreatedParseResult : null,
                IsAdmin = (accessTokenClaims?.IsAdmin != null && Boolean.TryParse(accessTokenClaims.IsAdmin, out bool isAdminParseResult)) ? isAdminParseResult : null
            };
        }

        public async Task<CurrentUserWithHeaders> GetCurrentUserWithHeaders()
        {
            var headerValues = await GetHeaders();

            if (string.IsNullOrEmpty(headerValues?.AccessToken) || (!headerValues?.AccessToken?.StartsWith("Bearer") ?? true))
                return new();

            AccessTokenClaimsData? accessTokenClaims = await _tokenService.GetAccessTokenClaims(headerValues.AccessToken);
            RefreshTokenClaimsData? refreshTokenClaims = !string.IsNullOrEmpty(headerValues?.RefreshToken) ? await _tokenService.GetRefreshTokenClaims(headerValues.RefreshToken) : null;

            if (accessTokenClaims?.UserId != null)
            {
                var accessTokenId = await _cacheService.GetValueAsync<Guid?>(accessTokenClaims.UserId);
                if (accessTokenId == default || !(Guid.TryParse(accessTokenClaims.AccessTokenId, out Guid parsedAccessTokenId)) || accessTokenId != parsedAccessTokenId)
                    return new();
            }

            return new()
            {
                UserId = (accessTokenClaims?.UserId != null && Guid.TryParse(accessTokenClaims.UserId, out Guid parsedUserId)) ? parsedUserId : null,
                FirstName = accessTokenClaims?.FirstName,
                LastName = accessTokenClaims?.LastName,
                Username = accessTokenClaims?.Username,
                EmailAddress = accessTokenClaims?.EmailAddress,
                UserStatus = (accessTokenClaims?.UserStatus != null && Int32.TryParse(accessTokenClaims.UserId, out int parsedUserStatus)) ? ((UserStatusType)parsedUserStatus) : null,
                LanguageCode = accessTokenClaims?.LanguageCode,
                UserDateCreated = (accessTokenClaims?.UserDateCreated != null && DateTime.TryParse(accessTokenClaims.UserId, CultureInfo.InvariantCulture, out DateTime parsedUserDateCreated)) ? parsedUserDateCreated : null,
                IsAdmin = (accessTokenClaims?.IsAdmin != null && Boolean.TryParse(accessTokenClaims.IsAdmin, out bool parsedIsAdmin)) ? parsedIsAdmin : null,
                AccessToken = !string.IsNullOrEmpty(headerValues?.AccessToken) ? headerValues.AccessToken : null,
                RefreshToken = !string.IsNullOrEmpty(headerValues?.RefreshToken) ? headerValues.RefreshToken : null,
                RefreshTokenId = Guid.TryParse(refreshTokenClaims?.RefreshTokenId, out Guid parsedRefreshTokenId) ? parsedRefreshTokenId : null
            };
        }

        public async Task<AccessTokenData> GeAccessTokenClaimsData()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.GetHeaderValue("Authorization")?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(accessToken))
                return null;

            AccessTokenClaimsData? accessTokenClaims = await _tokenService.GetAccessTokenClaims(accessToken);
            if (accessTokenClaims == null)
                return null;

            return new()
            {
                AccessTokenId = (accessTokenClaims?.AccessTokenId != null && Guid.TryParse(accessTokenClaims.AccessTokenId, out Guid parsedAccessTokenId)) ? parsedAccessTokenId : null,
                UserId = (accessTokenClaims?.UserId != null && Guid.TryParse(accessTokenClaims.UserId, out Guid parsedUserId)) ? parsedUserId : null,
                FirstName = accessTokenClaims?.FirstName,
                LastName = accessTokenClaims?.LastName,
                Username = accessTokenClaims?.Username,
                EmailAddress = accessTokenClaims?.EmailAddress,
                UserStatus = (accessTokenClaims?.UserStatus != null && Int32.TryParse(accessTokenClaims.UserId, out int parsedUserStatus)) ? ((UserStatusType)parsedUserStatus) : null,
                LanguageCode = accessTokenClaims?.LanguageCode,
                UserDateCreated = (accessTokenClaims?.UserDateCreated != null && DateTime.TryParse(accessTokenClaims.UserId, CultureInfo.InvariantCulture, out DateTime parsedUserDateCreated)) ? parsedUserDateCreated : null,
                IsAdmin = (accessTokenClaims?.IsAdmin != null && Boolean.TryParse(accessTokenClaims.IsAdmin, out bool parsedIsAdmin)) ? parsedIsAdmin : null
            };
        }

        public async Task<RefreshTokenData> GetRefreshTokenClaimsData()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.GetHeaderValue("refresh-token");
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            RefreshTokenClaimsData? refreshTokenClaims = await _tokenService.GetRefreshTokenClaims(refreshToken);
            if (refreshTokenClaims == null)
                return null;

            return new()
            {
                UserId = Guid.TryParse(refreshTokenClaims?.UserId, out Guid parsedUserId) ? parsedUserId : null,
                RefreshTokenId = Guid.TryParse(refreshTokenClaims?.RefreshTokenId, out Guid parsedRefreshTokenId) ? parsedRefreshTokenId : null
            };
        }

        public async Task<GetTokensFromHeaderData> GetHeaders()
        {
            return new()
            {
                AccessToken = _httpContextAccessor.HttpContext?.Request.GetHeaderValue("Authorization")?.Replace("Bearer ", ""),
                RefreshToken = _httpContextAccessor.HttpContext?.Request.GetHeaderValue("refresh-token")
            };
        }
    }
}
