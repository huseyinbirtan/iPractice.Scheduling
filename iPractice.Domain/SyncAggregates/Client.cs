using iPractice.Scheduling.Domain.ScheduleAggregate;

namespace iPractice.Scheduling.Domain.SyncAggregates
{
    public class Client
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Psychologist> Psychologists { get; set; }

        private readonly List<Appointment> appointments = new List<Appointment>();
        public IEnumerable<Appointment> Appointments => appointments.AsReadOnly();
    }
}