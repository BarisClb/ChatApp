using ChatApp.Domain.Entities.Common;

namespace ChatApp.Domain.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<User> User { get; set; }
    }
}
