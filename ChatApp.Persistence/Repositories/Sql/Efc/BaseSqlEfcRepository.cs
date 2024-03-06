using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Domain.Entities.Common;
using ChatApp.Domain.Enums;
using ChatApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class BaseSqlEfcRepository<TEntity> : IBaseSqlEfcRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ChatAppDbContext _context;
        protected readonly DbSet<TEntity> _entity;

        public BaseSqlEfcRepository(ChatAppDbContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }


        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityState = await _entity.AddAsync(entity);
            if (entityState.State != EntityState.Added)
                return null;
            var added = await _context.SaveChangesAsync();
            return added > 0 ? entityState.Entity : null;
        }

        public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            return await _context.SaveChangesAsync();
        }


        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entityState = _entity.Update(entity);
            if (entityState.State != EntityState.Modified)
                return null;
            var updated = await _context.SaveChangesAsync();
            return updated > 0 ? entityState.Entity : null;
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _entity.UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }


        //public async Task<bool> DeleteAsync(int entityId)
        //{
        //    var entity = await _entity.FindAsync(entityId);
        //    if (entity != null)
        //    {
        //        var entityState = _entity.Remove(entity);
        //        if (entityState.State != EntityState.Deleted)
        //            return false;
        //        return await _context.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}

        // EFC7
        public async Task<bool> DeleteAsync(int entityId)
        {
            return (await _entity.Where(x => x.Id == entityId).ExecuteDeleteAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var entityState = _entity.Remove(entity);
            if (entityState.State != EntityState.Deleted)
                return false;
            return await _context.SaveChangesAsync() > 0;
        }

        //public async Task<int> DeleteRangeAsync(IEnumerable<int> entities)
        //{
        //    foreach (var entityId in entities)
        //    {
        //        var entity = await _entity.FindAsync(entityId);
        //        if (entity != null)
        //            _entity.Remove(entity);
        //    }
        //    return await _context.SaveChangesAsync();
        //}

        // EFC7
        public async Task<int> DeleteRangeAsync(IEnumerable<int> entities)
        {
            return await _entity.Where(x => entities.Contains(x.Id)).ExecuteDeleteAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }


        public async Task<bool> DisableByIdAsync(int entityId)
        {
            return await _entity.Where(e => e.Id == entityId).ExecuteUpdateAsync(e => e.SetProperty(e => e.Status, EntityStatusType.Disabled)) > 0;
        }

        public async Task<int> DisableRangeByIdAsync(IEnumerable<int> entities)
        {
            return await _entity.Where(e => entities.Contains(e.Id)).ExecuteUpdateAsync(e => e.SetProperty(e => e.Status, EntityStatusType.Disabled));
        }

        // Helpers

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
