using AutoMapper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ChatApp.Application.Commands.UserToken
{
    public class UserTokenCreateCommand : IRequest<ApiResponse<NoContent>>
    {
        public Guid UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class UserTokenCreateCommandHandler : IRequestHandler<UserTokenCreateCommand, ApiResponse<NoContent>>
    {
        private readonly IUserTokenSqlEfcRepository _userTokenSqlEfcRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public UserTokenCreateCommandHandler(IUserTokenSqlEfcRepository userTokenSqlEfcRepository, IMapper mapper, IStringLocalizer<LocalizationResources> localization)
        {
            _userTokenSqlEfcRepository = userTokenSqlEfcRepository ?? throw new ArgumentNullException(nameof(userTokenSqlEfcRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }


        public async Task<ApiResponse<NoContent>> Handle(UserTokenCreateCommand request, CancellationToken cancellationToken)
        {
            var newUserToken = _mapper.Map<Domain.Entities.UserToken>(request);
            var result = await _userTokenSqlEfcRepository.AddAsync(newUserToken);

            if (result == null)
                return ApiResponse<NoContent>.Fail(string.Format(_localization[LocalizationKeys.FailedToCreateFollowingEntity], "UserToken"), (int)HttpStatusCode.InternalServerError);

            return ApiResponse<NoContent>.Success((int)HttpStatusCode.Created);
        }
    }
}
