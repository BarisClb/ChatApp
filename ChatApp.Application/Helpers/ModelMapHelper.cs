using ChatApp.Application.Commands.User;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Queries.User;

namespace ChatApp.Application.Helpers
{
    public static class ModelMapHelper
    {
        public static (GenerateTokenUserInfo userInfo, GenerateTokenUserRoleInfo userRoleInfo) MapUserAndUserRoleInfo(GetUserAndUserRoleByUserIdQueryData userAndUserRole)
        {
            GenerateTokenUserInfo userInfo = new()
            {
                Id = userAndUserRole.UserId,
                UserStatus = userAndUserRole.UserUserStatus,
                FirstName = userAndUserRole.FirstName,
                LastName = userAndUserRole.LastName,
                EmailAddress = userAndUserRole.EmailAddress,
                Username = userAndUserRole.Username,
                LanguageCode = userAndUserRole.LanguageCode,
                DateCreated = userAndUserRole.UserDateCreated
            };
            GenerateTokenUserRoleInfo userRoleInfo = new()
            {
                IsAdmin = userAndUserRole.IsAdmin
            };
            return (userInfo, userRoleInfo);
        }

        public static UserSessionResponse MapTokenDataToUserSessionResponse(GenerateTokenData tokenData)
        {
            return new()
            {
                UserId = Int32.TryParse(tokenData?.AccessTokenClaims?.UserId, out int parsedUserId) ? parsedUserId : null,
                UserStatus = Int32.TryParse(tokenData?.AccessTokenClaims?.UserStatus, out int parsedUserStatus) ? parsedUserStatus : null,
                FirstName = tokenData?.AccessTokenClaims?.FirstName,
                LastName = tokenData?.AccessTokenClaims?.LastName,
                Username = tokenData?.AccessTokenClaims?.Username,
                EmailAddress = tokenData?.AccessTokenClaims?.EmailAddress,
                LanguageCode = tokenData?.AccessTokenClaims?.LanguageCode,
                UserDateCreated = DateTime.TryParse(tokenData?.AccessTokenClaims?.UserDateCreated, out DateTime parsedUserDateCreated) ? parsedUserDateCreated : null,
                IsAdmin = bool.TryParse(tokenData?.AccessTokenClaims?.IsAdmin, out bool parsedIsAdmin) ? parsedIsAdmin : null,
                AccessToken = tokenData?.AccessToken,
                RefreshToken = tokenData?.RefreshToken,
            };
        }
    }
}
