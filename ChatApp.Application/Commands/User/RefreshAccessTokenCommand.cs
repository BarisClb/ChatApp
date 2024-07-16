using AutoMapper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Text;

namespace ChatApp.Application.Commands.User
{
    public class RefreshAccessTokenCommand : IRequest<ApiResponse<UserSessionResponse>>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, ApiResponse<UserSessionResponse>>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<LocalizationResources> _localizer;
        private readonly IMapper _mapper;

        public RefreshAccessTokenCommandHandler(IBaseSqlDapperRepository sqlDapperRepository, ITokenService tokenService, IStringLocalizer<LocalizationResources> localizer, IMapper mapper)
        {
            _sqlDapperRepository = sqlDapperRepository ?? throw new ArgumentNullException(nameof(sqlDapperRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<ApiResponse<UserSessionResponse>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken == null)
                throw new AuthorizationException() { PublicErrorMessage = _localizer.GetString(LocalizationKeys.SessionTimedOut), Method = "RefreshAccessTokenCommand" };

            var refreshTokenClaims = await _tokenService.GetRefreshTokenClaims(request.RefreshToken);
            if (refreshTokenClaims == null)
                throw new AuthorizationException() { PublicErrorMessage = _localizer.GetString(LocalizationKeys.SessionTimedOut), Method = "RefreshAccessTokenCommand" };

            // TODO:

            throw new NotImplementedException();
        }
    }
}
