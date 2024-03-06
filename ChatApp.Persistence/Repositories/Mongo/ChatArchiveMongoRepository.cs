using ChatApp.Application.Interfaces.Repositories.Mongo;
using ChatApp.Domain.Entities.Mongo;
using ChatApp.Persistence.Repositories.Mongo.Common;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Persistence.Repositories.Mongo
{
    public class ChatArchiveMongoRepository : BaseMongoRepository<ChatArchive>, IChatArchiveMongoRepository
    {
        public ChatArchiveMongoRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("MongoDb") ?? throw new Exception("ConnectionString was not provided for ChatArchiveMongoRepository."),
                  configuration.GetSection("MongoSettings:DatabaseName").Value ?? throw new Exception("DatabaseName was not provided for ChatArchiveMongoRepository."),
                  configuration.GetSection("MongoSettings:ChatArchiveCollectionName").Value ?? throw new Exception("ChatArchiveCollectionName was not provided for ChatArchiveMongoRepository."))
        { }
    }
}
