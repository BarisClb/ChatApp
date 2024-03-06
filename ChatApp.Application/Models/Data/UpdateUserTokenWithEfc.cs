using ChatApp.Domain.Enums;

namespace ChatApp.Application.Models.Data
{
    public class UpdateUserTokenWithEfc
    {
        public int Id { get; set; }
        public EntityStatusType? Status { get; set; }
        public Guid? RefreshTokenId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
