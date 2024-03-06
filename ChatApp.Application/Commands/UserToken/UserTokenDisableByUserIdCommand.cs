using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using MediatR;

namespace ChatApp.Application.Commands.UserToken
{
    public class UserTokenDisableByUserIdCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }

    public class UserTokenDisableByUserIdCommandHandler : IRequestHandler<UserTokenDisableByUserIdCommand, bool>
    {
        private readonly IUserTokenSqlEfcRepository _userTokenSqlEfcRepository;

        public UserTokenDisableByUserIdCommandHandler(IUserTokenSqlEfcRepository userTokenSqlEfcRepository)
        {
            _userTokenSqlEfcRepository = userTokenSqlEfcRepository ?? throw new ArgumentNullException(nameof(userTokenSqlEfcRepository));
        }


        public async Task<bool> Handle(UserTokenDisableByUserIdCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return new();

            return (await _userTokenSqlEfcRepository.UserTokenDisableByUserId(request.UserId)) > 0;
        }
    }
}
