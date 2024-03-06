using AutoMapper;
using ChatApp.Application.Commands.UserToken;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Models.Settings;
using ChatApp.Application.Queries.User;
using ChatApp.Application.Queries.UserRole;
using ChatApp.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Infrastructure.Jwt
{
    public class JwtService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(IOptions<JwtSettings> jwtSettings, IMapper mapper, IMediator mediator, ICacheService cacheService, IStringLocalizer<LocalizationResources> localization, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        public async Task<GenerateTokenData> GenerateTokens(Guid userId, Guid? refreshTokenId = default, GenerateTokenUserInfo? userInfo = default, GenerateTokenUserRoleInfo? userRoleInfo = default)
        {
            var accessTokenExpirationDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            var refreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationMinutes);

            var accessToken = await GenerateAccessToken(userId, userInfo, userRoleInfo, accessTokenExpirationDate);
            var refreshToken = await GenerateRefreshToken(userId, refreshTokenId ?? Guid.NewGuid(), refreshTokenExpirationDate);
            await _mediator.Send(new UserTokenRefreshOrCreateCommand() { UserId = userId, RefreshTokenId = refreshToken.RefreshTokenId, ExpirationDate = refreshTokenExpirationDate, IssueDate = DateTime.UtcNow });

            return new() { AccessToken = accessToken.AccessToken, RefreshToken = refreshToken.RefreshToken, AccessTokenClaims = accessToken.AccessTokenClaimsData };
        }

        public async Task<GenerateAccessTokenData> GenerateAccessToken(Guid userId, GenerateTokenUserInfo? userInfo = default, GenerateTokenUserRoleInfo? userRoleInfo = default, DateTime? tokenExpirationDate = default)
        {
            if (userInfo == null && userRoleInfo == null)
            {
                var userAndUserRoleInfoResponse = await _mediator.Send(new GetUserAndUserRoleByUserIdQuery() { UserId = userId }) ?? throw new ApiException() { OverrideLogMessage = $"Null Response from GetUserAndUserRoleByUserIdQuery at 'JwtService.GenerateAccessToken'. Request: 'userId='{userId}', userInfo='{JsonConvert.SerializeObject(userInfo)}', userRoleInfo='{JsonConvert.SerializeObject(userRoleInfo)}', tokenExpirationDate='{JsonConvert.SerializeObject(userInfo)}''." };
                var mappedUserAndUserRoleInfo = ModelMapHelper.MapUserAndUserRoleInfo(userAndUserRoleInfoResponse);
                userInfo = mappedUserAndUserRoleInfo.userInfo;
                userRoleInfo = mappedUserAndUserRoleInfo.userRoleInfo;
            }
            else
            {
                if (userInfo == null)
                {
                    var userResponse = await _mediator.Send(new GetUserByIdForTokenQuery() { UserId = userId }) ?? throw new ApiException() { OverrideLogMessage = $"Null Response from GetUserByIdForTokenQuery at 'JwtService.GenerateAccessToken'. Request: 'userId='{userId}', userInfo='{JsonConvert.SerializeObject(userInfo)}', userRoleInfo='{JsonConvert.SerializeObject(userRoleInfo)}', tokenExpirationDate='{JsonConvert.SerializeObject(userInfo)}''." };
                    userInfo = _mapper.Map<GenerateTokenUserInfo>(userResponse);
                }
                if (userRoleInfo == null)
                {
                    var userRole = await _mediator.Send(new GetUserRoleByUserIdQuery() { UserId = userId }) ?? throw new ApiException() { OverrideLogMessage = $"Null Response from GetUserRoleByUserIdQuery at 'JwtService.GenerateAccessToken'. Request: 'userId='{userId}', userInfo='{JsonConvert.SerializeObject(userInfo)}', userRoleInfo='{JsonConvert.SerializeObject(userRoleInfo)}', tokenExpirationDate='{JsonConvert.SerializeObject(userInfo)}''." };
                    userRoleInfo = _mapper.Map<GenerateTokenUserRoleInfo>(userRole);
                }
            }

            var accessTokenId = await _cacheService.GetValueAsync<Guid?>(string.Format(CacheHelper.AccessTokenIdKey, userId)) ?? Guid.NewGuid();
            // Even if the key already exists, Refresh Cache Expiration Date so it gets deleted if its unused for x amount of time (example: same as Refresh Token Expiration time)
            await _cacheService.SetValueAsync<Guid>(string.Format(CacheHelper.AccessTokenIdKey, userId), accessTokenId, CacheHelper.AccessTokenIdExpirationTime);

            AccessTokenClaimsData accessTokenClaimsData = new()
            {
                AccessTokenId = accessTokenId.ToString(),
                UserId = userInfo.Id.ToString(),
                UserStatus = ((int)userInfo.UserStatus).ToString(CultureInfo.InvariantCulture),
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                EmailAddress = userInfo.EmailAddress,
                Username = userInfo.Username,
                UserDateCreated = userInfo.DateCreated.ToString(CultureInfo.InvariantCulture),
                LanguageCode = userInfo.LanguageCode,
                IsAdmin = userRoleInfo.IsAdmin.ToString(CultureInfo.InvariantCulture)
            };

            var tokenClaims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer, ClaimValueTypes.String, _jwtSettings.Issuer),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, _jwtSettings.Issuer),
                new("AccessTokenId", accessTokenId.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer),
                new("UserId", accessTokenClaimsData.UserId, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("UserStatus", accessTokenClaimsData.UserStatus, ClaimValueTypes.Integer32, _jwtSettings.Issuer),
                new("FirstName", accessTokenClaimsData.FirstName, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("LastName", accessTokenClaimsData.LastName, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("EmailAddress", accessTokenClaimsData.EmailAddress, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("Username", accessTokenClaimsData.Username, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("UserDateCreated", accessTokenClaimsData.UserDateCreated, ClaimValueTypes.DateTime, _jwtSettings.Issuer),
                new("LanguageCode", accessTokenClaimsData.LanguageCode, ClaimValueTypes.String, _jwtSettings.Issuer),
                new("IsAdmin", accessTokenClaimsData.IsAdmin, ClaimValueTypes.Boolean, _jwtSettings.Issuer)
            };

            var issuerKey = new SymmetricSecurityKey(await convertSecretKeyToSHA256(_jwtSettings.IssuerSigningKey));
            var signingCreds = new SigningCredentials(issuerKey, SecurityAlgorithms.HmacSha256);
            var encryptionCredentials = _jwtSettings.UseEncryption ? new EncryptingCredentials(new SymmetricSecurityKey(await convertSecretKeyToSHA512(_jwtSettings.EncryptionKey)), JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512) : null;
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateJwtSecurityToken(_jwtSettings.Issuer,
                                                                       _jwtSettings.Audience,
                                                                       new ClaimsIdentity(tokenClaims),
                                                                       DateTime.UtcNow,
                                                                       tokenExpirationDate ?? DateTime.UtcNow.AddHours(_jwtSettings.AccessTokenExpirationMinutes),
                                                                       DateTime.UtcNow,
                                                                       signingCreds,
                                                                       encryptionCredentials);

            return new() { AccessToken = jwtSecurityTokenHandler.WriteToken(token), AccessTokenClaimsData = accessTokenClaimsData };
        }

        public async Task<GenerateRefreshTokenData> GenerateRefreshToken(Guid userId, Guid? refreshTokenIdReq, DateTime? tokenExpirationDate = default)
        {
            Guid refreshTokenId = refreshTokenIdReq ?? Guid.NewGuid();

            var tokenClaims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer, ClaimValueTypes.String, _jwtSettings.Issuer),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, _jwtSettings.Issuer),
                new("UserId", userId.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer),
                new("RefreshTokenId", refreshTokenId.ToString(), ClaimValueTypes.String, _jwtSettings.Issuer)
            };

            var issuerKey = new SymmetricSecurityKey(await convertSecretKeyToSHA256(_jwtSettings.IssuerSigningKey));
            var signingCreds = new SigningCredentials(issuerKey, SecurityAlgorithms.HmacSha256);
            var encryptionCredentials = _jwtSettings.UseEncryption ? new EncryptingCredentials(new SymmetricSecurityKey(await convertSecretKeyToSHA512(_jwtSettings.EncryptionKey)), JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512) : null;
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateJwtSecurityToken(_jwtSettings.Issuer,
                                                                       _jwtSettings.Audience,
                                                                       new ClaimsIdentity(tokenClaims),
                                                                       DateTime.UtcNow,
                                                                       tokenExpirationDate ?? DateTime.UtcNow.AddHours(_jwtSettings.AccessTokenExpirationMinutes),
                                                                       DateTime.UtcNow,
                                                                       signingCreds,
                                                                       encryptionCredentials);
            return new() { RefreshToken = jwtSecurityTokenHandler.WriteToken(token), RefreshTokenId = refreshTokenId };
        }

        public async Task<RefreshTokenClaimsData> GetRefreshTokenClaims(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            ClaimsPrincipal? decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(refreshToken,
                    new TokenValidationParameters
                    {
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = _jwtSettings.Audience,
                        ValidateAudience = true,
                        IssuerSigningKey = new SymmetricSecurityKey(await convertSecretKeyToSHA256(_jwtSettings.IssuerSigningKey)),
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        TokenDecryptionKey = _jwtSettings.UseEncryption ? new SymmetricSecurityKey(await convertSecretKeyToSHA512(_jwtSettings.EncryptionKey)) : null
                    }, out _);
            }
            catch (Exception ex)
            {
                if (ex is SecurityTokenExpiredException)
                    throw new AuthorizationException() { PublicErrorMessage = _localization.GetString(LocalizationKeys.SessionTimedOut), Method = "JwtService.GetRefreshTokenClaims", ErrorCode = (int)HttpStatusCode.Unauthorized };
                throw new ApiException() { ErrorCode = (int)HttpStatusCode.InternalServerError, Method = "JwtService.GetRefreshTokenClaims", OriginalException = ex };
            }

            return new()
            {
                UserId = decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "UserId", StringComparison.Ordinal))?.Value,
                RefreshTokenId = decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "RefreshTokenId", StringComparison.Ordinal))?.Value
            };
        }

        public async Task<AccessTokenClaimsData> GetAccessTokenClaims(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return null;

            ClaimsPrincipal? decodedAccessTokenPrincipal = null;
            try
            {
                decodedAccessTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(accessToken,
                new TokenValidationParameters
                {
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(await convertSecretKeyToSHA256(_jwtSettings.IssuerSigningKey)),
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    TokenDecryptionKey = _jwtSettings.UseEncryption ? new SymmetricSecurityKey(await convertSecretKeyToSHA512(_jwtSettings.EncryptionKey)) : null
                }, out _);
            }
            catch (Exception ex)
            {
                if (ex is SecurityTokenExpiredException)
                    throw new AuthorizationException() { PublicErrorMessage = _localization.GetString(LocalizationKeys.SessionTimedOut), Method = "JwtService.GetAccessTokenClaims", ErrorCode = (int)HttpStatusCode.Unauthorized };
                throw new ApiException() { ErrorCode = (int)HttpStatusCode.InternalServerError, Method = "JwtService.GetAccessTokenClaims", OriginalException = ex };
            }

            AccessTokenClaimsData claimsData = new()
            {
                AccessTokenId = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "AccessTokenId", StringComparison.Ordinal))?.Value,
                UserId = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "UserId", StringComparison.Ordinal))?.Value,
                UserStatus = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "UserStatus", StringComparison.Ordinal))?.Value,
                FirstName = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "FirstName", StringComparison.Ordinal))?.Value,
                LastName = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "LastName", StringComparison.Ordinal))?.Value,
                EmailAddress = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "EmailAddress", StringComparison.Ordinal))?.Value,
                Username = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "Username", StringComparison.Ordinal))?.Value,
                UserDateCreated = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "UserDateCreated", StringComparison.Ordinal))?.Value,
                LanguageCode = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "LanguageCode", StringComparison.Ordinal))?.Value,
                IsAdmin = decodedAccessTokenPrincipal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, "IsAdmin", StringComparison.Ordinal))?.Value
            };

            var accessTokenId = await _cacheService.GetValueAsync<Guid?>(string.Format(CacheHelper.AccessTokenIdKey, claimsData.UserId));
            if (accessTokenId == null || !(Guid.TryParse(claimsData.AccessTokenId, out Guid claimsAccessTokenId)) || accessTokenId != claimsAccessTokenId)
                throw new AuthorizationException() { PublicErrorMessage = _localization.GetString(LocalizationKeys.InvalidToken), Method = "HttpRequestHelper.GetCurrentUser", ErrorCode = (int)HttpStatusCode.Unauthorized };

            return claimsData;
        }


        private async Task<byte[]> convertSecretKeyToSHA256(string secretKey)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);
            using SHA256 sha256 = SHA256.Create();
            var result = sha256.ComputeHash(secretBytes);
            return result;
        }

        private async Task<byte[]> convertSecretKeyToSHA512(string secretKey)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);
            using SHA512 sha512 = SHA512.Create();
            var result = sha512.ComputeHash(secretBytes);
            return result;
        }

        public async Task<ApiResponse<UserSessionResponse>> RefreshAccessToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.GetHeaderValue("refresh-token");
            if (string.IsNullOrEmpty(refreshToken))
                throw new AuthorizationException() { PublicErrorMessage = _localization.GetString(LocalizationKeys.SessionTimedOut), Method = "JwtService.RefreshAccessToken", ErrorCode = (int)HttpStatusCode.Unauthorized };
            var refreshTokenClaims = await GetRefreshTokenClaims(refreshToken);
            var accessToken = await GenerateAccessToken(Guid.TryParse(refreshTokenClaims.UserId, out Guid parsedUserId) ? parsedUserId : Guid.Empty);
            return ApiResponse<UserSessionResponse>.Success(ModelMapHelper.MapTokenDataToUserSessionResponse(new() { AccessToken = accessToken?.AccessToken, AccessTokenClaims = accessToken.AccessTokenClaimsData, RefreshToken = refreshToken }), (int)HttpStatusCode.OK);
        }
    }
}
