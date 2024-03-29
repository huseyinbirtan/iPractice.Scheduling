namespace iPractice.SharedKernel.Interfaces
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull;

        Task<List<T>> ListAsync();
    }
}
