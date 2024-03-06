using ChatApp.Application.Interfaces.Repositories.Mongo.Common;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ChatApp.Persistence.Repositories.Mongo.Common
{
    public class BaseMongoRepository<TEntity> : IBaseMongoRepository<TEntity>
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public BaseMongoRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TEntity>(collectionName);
        }


        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await _collection.FindAsync(predicate)).ToList();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await (await _collection.FindAsync(predicate)).FirstOrDefaultAsync();
        }
    }
}
