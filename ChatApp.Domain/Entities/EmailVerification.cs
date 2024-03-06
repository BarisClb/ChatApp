using ChatApp.Domain.Entities.Common;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class EmailVerification : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public EmailVerificationType EmailVerificationType { get; set; }
        public string VerificationCode { get; set; }
        public Guid VerificationId { get; set; }
    }
}
