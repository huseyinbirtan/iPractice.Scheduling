using iPractice.SharedKernel.BaseClasses;

namespace iPractice.Scheduling.Domain.Events
{
    public class AppointmentScheduledEvent : BaseDomainEvent
    {
        public long ClientId { get; set; }
        public long PsychologistId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentScheduledEvent(long clientId, long psychologistId, DateTime startTime, DateTime endTime)
        {
            ClientId = clientId;
            PsychologistId = psychologistId;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
