using iPractice.Scheduling.Domain.ScheduleAggregate;

namespace iPractice.Scheduling.Domain.SyncAggregates
{
    public class Psychologist
    {
        public long Id { get; set; }
        public string Name { get; set; }

        private readonly List<Schedule> schedules = new List<Schedule>();
        public IEnumerable<Schedule> Schedules => schedules.AsReadOnly();

        public List<Client> Clients { get; set; }
    }
}