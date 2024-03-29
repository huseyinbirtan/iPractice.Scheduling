using iPractice.Scheduling.Domain.SyncAggregates;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace iPractice.DataAccess.Repositories
{
    public class ClientReadRepository : Repository<Client>, IClientReadRepository
    {
        public ClientReadRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Client> GetClientWithPsychologists(long clientId)
        {
            return await dbContext.Set<Client>().Include(a=>a.Psychologists).FirstOrDefaultAsync(c=>c.Id == clientId);
        }
    }
}
