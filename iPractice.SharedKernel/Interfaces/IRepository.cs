namespace iPractice.SharedKernel.Interfaces
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
