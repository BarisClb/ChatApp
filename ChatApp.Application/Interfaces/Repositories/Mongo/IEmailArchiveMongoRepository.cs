using ChatApp.Application.Interfaces.Repositories.Mongo.Common;
using ChatApp.Domain.Entities.Mongo;

namespace ChatApp.Application.Interfaces.Repositories.Mongo
{
    public interface IEmailArchiveMongoRepository : IBaseMongoRepository<EmailArchive>
    { }
}
