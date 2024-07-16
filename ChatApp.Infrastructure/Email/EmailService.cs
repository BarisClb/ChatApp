using ChatApp.Application.Commands.EmailArchive;
using ChatApp.Application.Commands.EmailVerification;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Models.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net;
using System.Net.Mail;

namespace ChatApp.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailServiceSettings> _emailServiceSettings;
        private readonly IMediator _mediator;

        public EmailService(IOptions<EmailServiceSettings> emailSettings, IMediator mediator)
        {
            _emailServiceSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task SendUserActivationEmail(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    throw new Exception("Invalid UserId for SendUserActivationEmail.");

                //var emailVerificationResponse = await _mediator.Send(new EmailVerificationCreateCommand() { UserId = userId, EmailVerificationType = Domain.Enums.EmailVerificationType.UserActivation });
                ApiResponse<EmailVerificationCreateCommandResponse> emailVerificationResponse = new() { Data = new() { FirstName = "Baris", LastName = "Celebi", EmailAddress = "barisclb1903@gmail.com", VerificationCode = "123", VerificationId = Guid.Empty, LanguageCode = "" } };
                if (!emailVerificationResponse.IsSuccess)
                    throw new Exception("EmailVerificationCreate failed for SendUserActivationEmail.");

                string subject = string.Format(await EmailHelper.GetUserActivationEmailSubject(emailVerificationResponse.Data.LanguageCode));
                string text = string.Format(await EmailHelper.GetUserActivationEmailBody(emailVerificationResponse.Data.LanguageCode),
                        emailVerificationResponse.Data.FirstName + " " + emailVerificationResponse.Data.LastName, emailVerificationResponse.Data.VerificationCode,
                        _emailServiceSettings.Value.DisableEmailUrl, "?emailAddress=", emailVerificationResponse.Data.EmailAddress, "&referenceId=", emailVerificationResponse.Data.VerificationId);

                try { await sendEmail(emailVerificationResponse.Data.EmailAddress, subject, text); }
                catch (Exception ex) { throw new ApiException() { OriginalException = ex, LogException = true, Method = "SendUserActivationEmail", ErrorCode = (int)HttpStatusCode.InternalServerError }; }

                try { await _mediator.Send(new EmailArchiveCreateCommand() { EmailSentTo = emailVerificationResponse.Data.EmailAddress, EmailSubject = subject, EmailBody = text }); }
                catch (Exception ex) { throw new ApiException() { OriginalException = ex, LogException = true, Method = "SendUserActivationEmail", ErrorCode = (int)HttpStatusCode.InternalServerError }; }
            }
            catch (Exception ex)
            {
                Log.Error(ex, LogHelper.ExceptionMessageTemplate, "SendUserActivationEmail", new { UserId = userId }, ex.Message);
            }
        }


        private async Task sendEmail(string receiverEmailAddress, string subject, string text)
        {
            if (string.IsNullOrEmpty(_emailServiceSettings.Value.ServiceEmailAddress) || string.IsNullOrEmpty(_emailServiceSettings.Value.ServiceEmailPassword))
            {
                Log.Error($"ServiceEmail or ServiceEmailPassword was not provided for EmailService-sendEmail Method. Following Email was not send to '{receiverEmailAddress}': {subject} - {text}");
                return;
            }

            MailMessage mail = new()
            {
                From = new MailAddress(_emailServiceSettings.Value.ServiceEmailAddress ?? ""),
                Subject = subject,
                Body = text,
                IsBodyHtml = true
            };

            mail.To.Add(receiverEmailAddress);

            SmtpClient smtp = new()
            {
                Credentials = new NetworkCredential(_emailServiceSettings.Value.ServiceEmailAddress ?? "", _emailServiceSettings.Value.ServiceEmailPassword ?? ""),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };

            smtp.Send(mail);
        }
    }
}
