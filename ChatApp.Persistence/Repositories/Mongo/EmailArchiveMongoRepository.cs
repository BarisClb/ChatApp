using ChatApp.Application.Interfaces.Repositories.Mongo;
using ChatApp.Domain.Entities.Mongo;
using ChatApp.Persistence.Repositories.Mongo.Common;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Persistence.Repositories.Mongo
{
    public class EmailArchiveMongoRepository : BaseMongoRepository<EmailArchive>, IEmailArchiveMongoRepository
    {
        public EmailArchiveMongoRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("MongoDb") ?? throw new Exception("ConnectionString was not provided for EmailArchiveMongoRepository."),
                  configuration.GetSection("MongoSettings:DatabaseName").Value ?? throw new Exception("DatabaseName was not provided for EmailArchiveMongoRepository."),
                  configuration.GetSection("MongoSettings:EmailArchiveCollectionName").Value ?? throw new Exception("EmailArchiveCollectionName was not provided for EmailArchiveMongoRepository."))
        { }
    }
}
