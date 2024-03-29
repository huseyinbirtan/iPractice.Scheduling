using iPractice.SharedKernel.Interfaces;

namespace iPractice.Scheduling.Domain.ScheduleAggregate
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<Schedule> GetByPsychologistIdAsync(long psychologistId);
    }
}
