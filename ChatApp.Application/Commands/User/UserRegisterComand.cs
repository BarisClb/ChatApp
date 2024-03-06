using AutoMapper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Models.Settings;
using ChatApp.Application.Resources;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace ChatApp.Application.Commands.User
{
    public class UserRegisterCommand : IRequest<ApiResponse<UserSessionResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LanguageCode { get; set; }
    }

    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, ApiResponse<UserSessionResponse>>
    {
        private readonly IUserSqlEfcRepository _userSqlEfcRepository;
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly ILanguageHelper _languageHelper;
        private readonly IUserRoleSqlEfcRepository _userRoleSqlEfcRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<ProjectSettings> _projectSettings;
        private readonly ITokenService _tokenService;

        public UserRegisterCommandHandler(IUserSqlEfcRepository userSqlEfcRepository, IBaseSqlDapperRepository baseSqlDapperRepository, IStringLocalizer<LocalizationResources> localizer, ILanguageHelper languageHelper, IUserRoleSqlEfcRepository userRoleSqlEfcRepository, IMapper mapper, IOptions<ProjectSettings> projectSettings, ITokenService tokenService)
        {
            _userSqlEfcRepository = userSqlEfcRepository ?? throw new ArgumentNullException(nameof(userSqlEfcRepository));
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _languageHelper = languageHelper ?? throw new ArgumentNullException(nameof(languageHelper));
            _userRoleSqlEfcRepository = userRoleSqlEfcRepository ?? throw new ArgumentNullException(nameof(userRoleSqlEfcRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }


        public async Task<ApiResponse<UserSessionResponse>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var duplicateFieldUser = await _sqlDapperRepository.QueryFirstOrDefaultAsync<UserRegisterCommandUniqueFields>(SqlQueryHelper.UserRegisterCommandUniqueFieldsQuery, new { /*EntityStatusTypeActive = (int)EntityStatusType.Active,*/ Username = request.Username, EmailAddress = request.EmailAddress });

            if (duplicateFieldUser?.EmailAddress == request.EmailAddress)
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserRegisterEmailIsNotUnique], (int)HttpStatusCode.BadRequest);

            if (duplicateFieldUser?.Username == request.Username)
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserRegisterUsernameIsNotUnique], (int)HttpStatusCode.BadRequest);

            var language = _languageHelper.GetLanguages.FirstOrDefault(language => language.Code == request.LanguageCode);
            if (language == null)
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserRegisterLanguageIsNotSupported], (int)HttpStatusCode.BadRequest);

            var newUserRequest = _mapper.Map<Domain.Entities.User>(request);
            newUserRequest.LanguageId = language.Id;
            newUserRequest.UserStatus = _projectSettings.Value.IsVerificationRequired ? UserStatusType.Pending : UserStatusType.Active;

            var newUser = await _userSqlEfcRepository.AddAsync(newUserRequest) ?? throw new ApiException() { OverrideLogMessage = $"Failed to create User at 'UserRegisterCommandHandler'. Request: '{JsonConvert.SerializeObject(request)}'" };

            try { var newUserRole = await _userRoleSqlEfcRepository.AddAsync(new UserRole() { UserId = newUser.Id, Status = EntityStatusType.Active, IsAdmin = false, DateCreated = DateTime.UtcNow }) ?? throw new Exception("Failed to Create UserRole."); }
            catch (Exception ex)
            {
                await _userSqlEfcRepository.DeleteAsync(newUser.Id);
                throw new ApiException() { ErrorCode = (int)HttpStatusCode.InternalServerError, Method = "UserRegisterCommandHandler", OriginalException = ex };
            }

            try
            {
                if (_projectSettings.Value.IsVerificationRequired)
                    BackgroundJob.Enqueue<IEmailService>(emailService => emailService.SendUserActivationEmail(newUser.Id));
            }
            catch (Exception ex) { Log.Error(ex, LogHelper.ExceptionMessageTemplate, "UserRegisterCommandHandler", JsonConvert.SerializeObject(request), ex.Message); }

            GenerateTokenData? tokens = null;
            try { tokens = await _tokenService.GenerateTokens(newUser.Id); }
            catch (Exception ex) { Log.Error(ex, LogHelper.ExceptionMessageTemplate, "UserRegisterCommandHandler", JsonConvert.SerializeObject(request), ex.Message); }

            return ApiResponse<UserSessionResponse>.Success(ModelMapHelper.MapTokenDataToUserSessionResponse(tokens), (int)HttpStatusCode.Created);
        }
    }


    public class UserRegisterCommandUniqueFields
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}
