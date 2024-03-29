using iPractice.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace iPractice.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbContext.Set<T>().RemoveRange(entities);
            await SaveChangesAsync();
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull
        {
            return await dbContext.Set<T>().FindAsync(new object[1] { id });
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
    }
}
