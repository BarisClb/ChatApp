using ChatApp.Application.Interfaces.Repositories.Mongo;
using ChatApp.Application.Models.Responses.Common;
using MediatR;
using System.Net;

namespace ChatApp.Application.Commands.EmailArchive
{
    public class EmailArchiveCreateCommand : IRequest<ApiResponse<NoContent>>
    {
        public string EmailSentTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }

    public class EmailArchiveCreateCommandHandler : IRequestHandler<EmailArchiveCreateCommand, ApiResponse<NoContent>>
    {
        private readonly IEmailArchiveMongoRepository _emailArchiveMongoRepository;

        public EmailArchiveCreateCommandHandler(IEmailArchiveMongoRepository emailArchiveMongoRepository)
        {
            _emailArchiveMongoRepository = emailArchiveMongoRepository ?? throw new ArgumentNullException(nameof(emailArchiveMongoRepository));
        }


        public async Task<ApiResponse<NoContent>> Handle(EmailArchiveCreateCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.EmailSentTo))
                throw new Exception("EmailSentTo was not provided for EmailArchiveCreateCommand.");
            if (string.IsNullOrEmpty(request.EmailSubject))
                throw new Exception("EmailSubject was not provided for EmailArchiveCreateCommand.");
            if (string.IsNullOrEmpty(request.EmailBody))
                throw new Exception("EmailBody was not provided for EmailArchiveCreateCommand.");

            await _emailArchiveMongoRepository.InsertAsync(new() { _id = Guid.NewGuid(), EmailSentTo = request.EmailSentTo, EmailSubject = request.EmailSubject, EmailBody = request.EmailBody, DateCreated = DateTime.UtcNow });
            return ApiResponse<NoContent>.Success((int)HttpStatusCode.Created);
        }
    }

}
