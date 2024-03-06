using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using MediatR;

namespace ChatApp.Application.Commands.UserToken
{
    public class UserTokenDisableByUserAndRefreshTokenIdCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
    }

    public class UserTokenDisableByUserAndRefreshTokenIdCommandHandler : IRequestHandler<UserTokenDisableByUserAndRefreshTokenIdCommand, bool>
    {
        private readonly IUserTokenSqlEfcRepository _userTokenSqlEfcRepository;

        public UserTokenDisableByUserAndRefreshTokenIdCommandHandler(IUserTokenSqlEfcRepository userTokenSqlEfcRepository)
        {
            _userTokenSqlEfcRepository = userTokenSqlEfcRepository ?? throw new ArgumentNullException(nameof(userTokenSqlEfcRepository));
        }


        public async Task<bool> Handle(UserTokenDisableByUserAndRefreshTokenIdCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty || request.RefreshTokenId == Guid.Empty)
                return new();

            return (await _userTokenSqlEfcRepository.UserTokenDisableByUserAndRefreshTokenId(request.UserId, request.RefreshTokenId)) > 0;
        }
    }
}
