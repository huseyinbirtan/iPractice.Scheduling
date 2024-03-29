using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.Scheduling.Domain.ValueObjects;
using iPractice.SharedKernel.BaseClasses;

namespace iPractice.Scheduling.Domain.ScheduleAggregate
{
    public class Appointment : BaseEntity<Guid>
    {
        public Guid AvailabilityId { get; set; }
        public Availability Availability { get; private set; }
        public long ClientId { get; set; }
        public Client Client { get; private set; }
        public TimeSlotValueObject TimeSlot { get; set; }
        private Appointment() { }
        public Appointment(Guid id, Guid availabilityId, long clientId, TimeSlotValueObject timeSlot)
        {
            Id = id == Guid.Empty ? throw FieldException(nameof(id)) : id;
            AvailabilityId = availabilityId == Guid.Empty ? throw FieldException(nameof(availabilityId)) : availabilityId;
            ClientId = clientId == 0 ? throw FieldException(nameof(clientId)) : clientId;
            TimeSlot = timeSlot == null ? throw FieldException(nameof(timeSlot)) : timeSlot;
        }
    }
}
