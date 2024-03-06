using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Settings;

namespace ChatApp.Application.Helpers
{
    public static class SqlQueryHelper
    {
        public static SqlDbChoiceType SqlDatabaseChoice { get; set; }

        public static string GetLanguageByUserIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT language.Id, language.Name, language.Code FROM User AS user INNER JOIN Language AS language ON language.Status = @EntityStatusTypeActive AND user.LanguageId = language.Id WHERE user.Status = @EntityStatusTypeActive AND user.Id = '@UserId' LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "language"."Id", "language"."Name", "language"."Code" FROM "User" AS "user" INNER JOIN "Language" AS "language" ON "language"."Status" = @EntityStatusTypeActive AND "user"."LanguageId" = "language"."Id" WHERE "user"."Status" = @EntityStatusTypeActive AND "user"."Id" = '@UserId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) language.Id, language.Name, language.Code FROM [User] AS u INNER JOIN Language AS language ON language.Status = @EntityStatusTypeActive AND u.LanguageId = language.Id WHERE u.Status = @EntityStatusTypeActive AND u.Id = @UserId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetLanguageByUserIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetUserAndUserRoleByUserIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT user.Id AS 'UserId', user.UserStatus AS 'UserUserStatus', user.FirstName, user.LastName, user.EmailAddress, user.Username, user.DateCreated AS 'UserDateCreated', userrole.IsAdmin, language.Code AS 'LanguageCode' FROM User as user INNER JOIN UserRole as userrole ON userrole.UserId = user.Id AND userrole.Status = @EntityStatusTypeActive INNER JOIN Language AS language ON language.Id = user.LanguageId AND language.Status = @EntityStatusTypeActive WHERE user.Status = @EntityStatusTypeActive AND Id = '@UserId' LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "user"."Id" AS "UserId", "user"."UserStatus" AS "UserUserStatus", "user"."FirstName", "user"."LastName", "user"."EmailAddress", "user"."Username", "user"."DateCreated" AS "UserDateCreated", "userrole"."IsAdmin", "language"."Code" AS "LanguageCode" FROM "User" as "user" INNER JOIN "UserRole" as "userrole" ON "userrole"."UserId" = "user"."Id" AND "userrole"."Status" = @EntityStatusTypeActive INNER JOIN "Language" AS "language" ON "language"."Id" = "user"."LanguageId" AND "language"."Status" = @EntityStatusTypeActive WHERE "user"."Status" = @EntityStatusTypeActive AND "user"."Id" = '@UserId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) u.Id AS 'UserId', u.UserStatus AS 'UserUserStatus', u.FirstName, u.LastName, u.EmailAddress, u.Username, u.DateCreated AS 'UserDateCreated', userrole.IsAdmin, language.Code AS 'LanguageCode' FROM [User] as u INNER JOIN UserRole as userrole ON userrole.UserId = u.Id AND userrole.Status = @EntityStatusTypeActive INNER JOIN Language AS language ON language.Id = u.LanguageId AND language.Status = @EntityStatusTypeActive WHERE u.Status = @EntityStatusTypeActive AND u.Id = @UserId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserAndUserRoleByUserIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetUserByIdForTokenQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT user.Id, user.FirstName, user.LastName, user.EmailAddress, user.Username, user.UserStatus, user.DateCreated, language.Code AS 'LanguageCode' FROM User AS user INNER JOIN Language AS language ON language.Id = user.LanguageId AND language.Status = @EntityStatusTypeActive WHERE user.Status = @EntityStatusTypeActive AND user.Id = '@UserId' LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "user"."Id", "user"."FirstName", "user"."LastName", "user"."EmailAddress", "user"."Username", "user"."UserStatus", "user"."DateCreated", "language"."Code" AS "LanguageCode" FROM "User" AS "user" INNER JOIN "Language" AS "language" ON "language"."Id" = "user"."LanguageId" AND "language"."Status" = @EntityStatusTypeActive WHERE "user"."Status" = @EntityStatusTypeActive AND "user"."Id" = '@UserId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) u.Id, u.FirstName, u.LastName, u.EmailAddress, u.Username, u.UserStatus, u.DateCreated, language.Code AS 'LanguageCode' FROM [User] AS u INNER JOIN Language AS language ON language.Id = u.LanguageId AND language.Status = @EntityStatusTypeActive WHERE u.Status = @EntityStatusTypeActive AND u.Id = @UserId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserByIdForTokenQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetUserByUsernameQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id, Status, FirstName, LastName, EmailAddress, Username, Password, UserStatus, DateCreated, DateUpdated FROM User WHERE Status = @EntityStatusTypeActive AND Username = @Username LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id", "Status", "FirstName", "LastName", "EmailAddress", "Username", "Password", "UserStatus", "DateCreated", "DateUpdated" FROM "User" WHERE "Status" = @EntityStatusTypeActive AND "Username" = @Username LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) Id, Status, FirstName, LastName, EmailAddress, Username, Password, UserStatus, DateCreated, DateUpdated FROM [User] WHERE Status = @EntityStatusTypeActive AND Username = @Username";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserByUsernameQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetUserRoleByUserIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT UserId, Status, IsAdmin FROM AccountRole WHERE Status = @EntityStatusTypeActive AND UserId = '@UserId' LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "UserId", "Status", "IsAdmin" FROM "AccountRole" WHERE "Status" = @EntityStatusTypeActive AND "UserId" = '@UserId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) UserId, Status, IsAdmin FROM AccountRole WHERE Status = @EntityStatusTypeActive AND UserId = @UserId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserRoleByUserIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetValidUserTokenByUserIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT UserId, RefreshTokenId, IssueDate, ExpirationDate, DateCreated FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = '@UserId' AND ExpirationDate > @DateTimeUtcNow LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "UserId", "RefreshTokenId", "IssueDate", "ExpirationDate", "DateCreated" FROM "UserToken" WHERE "Status" = @EntityStatusTypeActive AND "UserId" = '@UserId' AND "ExpirationDate" > '@DateTimeUtcNow' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) UserId, RefreshTokenId, IssueDate, ExpirationDate, DateCreated FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = @UserId AND ExpirationDate > @DateTimeUtcNow";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetValidUserTokenByUserIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetValidUserTokenByUserAndRefreshTokenIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT UserId, RefreshTokenId, IssueDate, ExpirationDate FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = '@UserId' AND RefreshTokenId = @RefreshTokenId AND ExpirationDate > @DateTimeUtcNow LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "UserId", "RefreshTokenId", "IssueDate", "ExpirationDate" FROM "UserToken" WHERE "Status" = @EntityStatusTypeActive AND "UserId" = '@UserId' AND "RefreshTokenId" = '@RefreshTokenId' AND "ExpirationDate" > '@DateTimeUtcNow' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) UserId, RefreshTokenId, IssueDate, ExpirationDate FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = @UserId AND RefreshTokenId = @RefreshTokenId AND ExpirationDate > @DateTimeUtcNow";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetValidUserTokenByUserAndRefreshTokenIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetLanguagesForLanguageHelperQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id, Name, Code FROM Language WHERE Status = @EntityStatusTypeActive";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id", "Name", "Code" FROM "Language" WHERE "Status" = @EntityStatusTypeActive""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT Id, Name, Code FROM Language WHERE Status = @EntityStatusTypeActive";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetLanguagesForLanguageHelperQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string UserLogInCommandQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id, Status, UserStatus FROM User WHERE {0} = @UserField AND Password = @Password LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id", "Status", "UserStatus" FROM "User" WHERE "{0}" = @UserField AND "Password" = @Password LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) Id, Status, UserStatus FROM [User] WHERE {0} = @UserField AND Password = @Password";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.UserLogInCommandQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string UserRegisterCommandUniqueFieldsQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT EmailAddress, Username FROM User WHERE (EmailAddress = @EmailAddress OR Username = @Username)";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "EmailAddress", "Username" FROM "User" WHERE ("EmailAddress" = @EmailAddress OR "Username" = @Username)""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT EmailAddress, Username FROM [User] WHERE (EmailAddress = @EmailAddress OR Username = @Username)";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.UserRegisterCommandUniqueFieldsQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string AnyLanguageExistsQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT 1 FROM Language WHERE Status = @EntityStatusTypeActive LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT 1 FROM "Language" WHERE "Status" = @EntityStatusTypeActive LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) 1 FROM Language WHERE Status = @EntityStatusTypeActive";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.AnyLanguageExistsQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string AnyUserExistsQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT 1 FROM User WHERE Status = @EntityStatusTypeActive LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT 1 FROM "User" WHERE "Status" = @EntityStatusTypeActive LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) 1 FROM [User] WHERE Status = @EntityStatusTypeActive";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.AnyUserExistsQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetUserTokenByUserAndRefreshTokenIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id, RefreshTokenId, ExpirationDate FROM UserToken WHERE UserId = '@UserId' AND RefreshTokenId = @RefreshTokenId LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id", "RefreshTokenId", "ExpirationDate" FROM "UserToken" WHERE "UserId" = '@UserId' AND "RefreshTokenId" = '@RefreshTokenId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) Id, RefreshTokenId, ExpirationDate FROM UserToken WHERE UserId = @UserId AND RefreshTokenId = @RefreshTokenId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserTokenByUserAndRefreshTokenIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string GetActiveUserTokenByUserIdRefreshTokenIdQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id, RefreshTokenId, ExpirationDate FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = '@UserId' AND RefreshTokenId = @RefreshTokenId LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id", "RefreshTokenId", "ExpirationDate" FROM "UserToken" WHERE "Status" = @EntityStatusTypeActive AND "UserId" = '@UserId' AND "RefreshTokenId" = '@RefreshTokenId' LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) Id, RefreshTokenId, ExpirationDate FROM UserToken WHERE Status = @EntityStatusTypeActive AND UserId = @UserId AND RefreshTokenId = @RefreshTokenId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.GetUserTokenByUserIdRefreshTokenIdQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }

        public static string UserTokenRefreshOrCreateQuery
        {
            get
            {
                if (SqlDatabaseChoice == SqlDbChoiceType.MySql)
                    return "SELECT Id FROM UserToken WHERE UserId = '@UserId' AND RefreshTokenId = @RefreshTokenId LIMIT 1";
                else if (SqlDatabaseChoice == SqlDbChoiceType.PostgreSql)
                    return """SELECT "Id" FROM "UserToken" WHERE "UserId" = '@UserId' AND "RefreshTokenId" = @RefreshTokenId LIMIT 1""";
                else if (SqlDatabaseChoice == SqlDbChoiceType.MsSql)
                    return "SELECT TOP(1) Id FROM UserToken WHERE UserId = @UserId AND RefreshTokenId = @RefreshTokenId";
                throw new ApiException() { OverrideLogMessage = $"'SqlQueryHelper.UserTokenRefreshOrCreateQuery' was not found for '{SqlDatabaseChoice}'.", LogException = true };
            }
        }
    }
}
