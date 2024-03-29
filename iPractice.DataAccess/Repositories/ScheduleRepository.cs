using iPractice.Scheduling.Domain.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iPractice.DataAccess.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Schedule> GetByPsychologistIdAsync(long psychologistId)
        {
            return await dbContext.Set<Schedule>()
                .Include(a => a.Availabilities.Where(a=>a.AvailabilityTimeSlot.StartTime >= DateTime.UtcNow ))
                .ThenInclude(a => a.Appointments.Where(a => a.TimeSlot.StartTime >= DateTime.UtcNow))
                .FirstOrDefaultAsync(a => a.PsychologistId == psychologistId);
        }
    }
}
