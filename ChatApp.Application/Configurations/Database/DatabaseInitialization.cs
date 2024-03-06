using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Models.Settings;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ChatApp.Application.Configurations.Database
{
    public class DatabaseInitialization : IDatabaseInitialization
    {
        private readonly IOptions<SqlDatabaseSeedingSettings> _sqlDatabaseSeedingSettings;
        private readonly IUserSqlEfcRepository _userSqlEfcRepository;
        private readonly IUserRoleSqlEfcRepository _userRoleSqlEfcRepository;
        private readonly ILanguageSqlEfcRepository _languageSqlEfcRepository;
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly IOptions<ProjectSettings> _projectSettings;

        public DatabaseInitialization(IOptions<SqlDatabaseSeedingSettings> sqlDatabaseSeedingSettings, IUserSqlEfcRepository userSqlEfcRepository, IUserRoleSqlEfcRepository userRoleSqlEfcRepository, ILanguageSqlEfcRepository languageSqlEfcRepository, IBaseSqlDapperRepository baseSqlDapperRepository, IOptions<ProjectSettings> projectSettings)
        {
            _sqlDatabaseSeedingSettings = sqlDatabaseSeedingSettings ?? throw new ArgumentNullException(nameof(sqlDatabaseSeedingSettings));
            _userSqlEfcRepository = userSqlEfcRepository ?? throw new ArgumentNullException(nameof(userSqlEfcRepository));
            _userRoleSqlEfcRepository = userRoleSqlEfcRepository ?? throw new ArgumentNullException(nameof(userRoleSqlEfcRepository));
            _languageSqlEfcRepository = languageSqlEfcRepository ?? throw new ArgumentNullException(nameof(languageSqlEfcRepository));
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
        }


        public async Task SeedSqlDatabase()
        {
            if (!_sqlDatabaseSeedingSettings.Value.SeedDatabase)
                return;

            if (!(await _sqlDapperRepository.QueryFirstOrDefaultAsync<bool?>(SqlQueryHelper.AnyLanguageExistsQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active }) ?? false) && _sqlDatabaseSeedingSettings.Value.SeedLanguages)
                await _languageSqlEfcRepository.AddRangeAsync(await getLanguagesToSeed());

            if (!(await _sqlDapperRepository.QueryFirstOrDefaultAsync<bool?>(SqlQueryHelper.AnyUserExistsQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active }) ?? false))
            {
                if (_sqlDatabaseSeedingSettings.Value.SeedAdmin)
                {
                    var adminAndAdminRole = await getAdminAndAdminRoleToSeed();
                    await _userSqlEfcRepository.AddAsync(adminAndAdminRole.user);
                    await _userRoleSqlEfcRepository.AddAsync(adminAndAdminRole.userRole);
                }

                if (_sqlDatabaseSeedingSettings.Value.SeedUsers)
                {
                    var userAndUserRoles = await getUsersAndUserRolesToSeed();
                    await _userSqlEfcRepository.AddRangeAsync(userAndUserRoles.users);
                    await _userRoleSqlEfcRepository.AddRangeAsync(userAndUserRoles.userRoles);
                }
            }
        }


        private static async Task<List<Language>> getLanguagesToSeed()
        {
            return new()
            {
                new()
                {
                     Name = "English",
                     Code = "en",
                     DateCreated = DateTime.UtcNow,
                     Status = EntityStatusType.Active
                },
                new()
                {
                     Name = "Turkish",
                     Code = "tr",
                     DateCreated = DateTime.UtcNow,
                     Status = EntityStatusType.Active
                }
            };
        }

        private async Task<(User user, UserRole userRole)> getAdminAndAdminRoleToSeed()
        {
            var hashedAdminPassword = SecurityHelper.GetSha256Hash(_sqlDatabaseSeedingSettings.Value.AdminPassword);
            Guid adminId = Guid.NewGuid();
            User admin = new()
            {
                Id = adminId,
                FirstName = _sqlDatabaseSeedingSettings.Value.AdminFirstName,
                LastName = _sqlDatabaseSeedingSettings.Value.AdminLastName,
                EmailAddress = _sqlDatabaseSeedingSettings.Value.AdminEmailAddress,
                Username = _sqlDatabaseSeedingSettings.Value.AdminUsername,
                Password = hashedAdminPassword,
                BirthDay = new DateTime(2000, 1, 1),
                DateCreated = DateTime.UtcNow,
                Status = EntityStatusType.Active,
                UserStatus = UserStatusType.Active,
                LanguageId = 1
            };
            UserRole adminRole = new()
            {
                UserId = adminId,
                IsAdmin = true,
                DateCreated = DateTime.UtcNow,
                Status = EntityStatusType.Active
            };

            return (admin, adminRole);
        }

        private async Task<(List<User> users, List<UserRole> userRoles)> getUsersAndUserRolesToSeed()
        {
            int userSequence = _sqlDatabaseSeedingSettings.Value.SeedAdmin == true ? 1 : 0;
            var hashedUserPassword = SecurityHelper.GetSha256Hash(_sqlDatabaseSeedingSettings.Value.UserPassword);
            var userEmailAddressPrefix = _sqlDatabaseSeedingSettings.Value.UserEmailAddress.Split('@')[0];
            List<User> users = new();
            List<UserRole> userRoles = new();
            List<(User user, UserRole userRole)> userAndUserRoles = new();
            for (var i = 1 + userSequence; i <= _sqlDatabaseSeedingSettings.Value.NumberOfUsersToSeed + userSequence; i++)
            {
                Guid userId = Guid.NewGuid();

                users.Add(new()
                {
                    Id = userId,
                    FirstName = _sqlDatabaseSeedingSettings.Value.UserFirstName + " " + i,
                    LastName = _sqlDatabaseSeedingSettings.Value.UserLastName + " " + i,
                    EmailAddress = _sqlDatabaseSeedingSettings.Value.UserEmailAddress.Replace(userEmailAddressPrefix, userEmailAddressPrefix + i),
                    Username = _sqlDatabaseSeedingSettings.Value.UserUsername + i,
                    Password = hashedUserPassword,
                    BirthDay = new DateTime(2000 + i, 1, 1),
                    DateCreated = DateTime.UtcNow,
                    Status = EntityStatusType.Active,
                    UserStatus = UserStatusType.Active,
                    LanguageId = 1
                });
                userRoles.Add(new()
                {
                    UserId = userId,
                    Status = EntityStatusType.Active,
                    IsAdmin = false,
                    DateCreated = DateTime.UtcNow
                });
            }

            return (users, userRoles);
        }
    }
}
