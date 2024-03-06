using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Commands.EmailVerification
{
    public class EmailVerificationCreateCommand : IRequest<ApiResponse<EmailVerificationCreateCommandResponse>>
    {
        public int UserId { get; set; }
        public EmailVerificationType EmailVerificationType { get; set; }
    }

    public class EmailVerificationCreateCommandHandler : IRequestHandler<EmailVerificationCreateCommand, ApiResponse<EmailVerificationCreateCommandResponse>>
    {
        private readonly IEmailVerificationSqlEfcRepository _emailVerificationSqlEfcRepository;
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public EmailVerificationCreateCommandHandler(IEmailVerificationSqlEfcRepository emailVerificationSqlEfcRepository, IBaseSqlDapperRepository baseSqlDapperRepository)
        {
            _emailVerificationSqlEfcRepository = emailVerificationSqlEfcRepository ?? throw new ArgumentNullException(nameof(emailVerificationSqlEfcRepository));
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
        }


        public async Task<ApiResponse<EmailVerificationCreateCommandResponse>> Handle(EmailVerificationCreateCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }

    public class EmailVerificationCreateCommandResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string VerificationCode { get; set; }
        public Guid VerificationId { get; set; }
        public EmailVerificationType EmailVerificationType { get; set; }
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; }
    }
}
