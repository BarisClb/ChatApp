using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public EntityStatusType Status { get; set; }
    }
}
