using iPractice.SharedKernel.Interfaces;

namespace iPractice.Scheduling.Domain.SyncAggregates
{
    public interface IClientReadRepository : IReadRepository<Client>
    {
        Task<Client> GetClientWithPsychologists(long clientId);
    }
}
