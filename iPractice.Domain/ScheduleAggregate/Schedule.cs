using iPractice.Scheduling.Domain.Exceptions;
using iPractice.SharedKernel.Interfaces;
using iPractice.Scheduling.Domain.ValueObjects;
using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.SharedKernel.BaseClasses;

namespace iPractice.Scheduling.Domain.ScheduleAggregate
{
    public class Schedule : BaseEntity<Guid>, IAggregateRoot
    {
        public long PsychologistId { get; set; }
        public Psychologist Psychologist { get; private set; }

        private readonly List<Availability> availabilities = new List<Availability>();
        public IEnumerable<Availability> Availabilities => availabilities.AsReadOnly();

        private Schedule() { }

        public Schedule(Guid id, long psychologistId)
        {
            Id = id == Guid.Empty ? throw FieldException(nameof(id)) : id;
            PsychologistId = psychologistId == 0 ? throw FieldException(nameof(psychologistId)) : psychologistId;
        }

        public Schedule CreateAvailability(Availability availability) 
        {
            if (availabilities.Any(a => a.AvailabilityTimeSlot.Overlaps(availability.AvailabilityTimeSlot)))
            {
                throw new OverlappingAvailablityException();
            }

            availabilities.Add(availability);
            return this;
        }

        public IEnumerable<TimeSlotValueObject> GetAvailableTimeSlots()
        {
            var scheduleTimeSlots = new List<TimeSlotValueObject>();

            foreach (var availability in availabilities)
            {
                scheduleTimeSlots.AddRange(availability.GetAvailableTimeSlots());
            }

            return scheduleTimeSlots;
        }

        public Availability GetAvailablityContainingTimeSlot(TimeSlotValueObject timeSlot)
        {
            return availabilities.FirstOrDefault(a => a.AvailabilityTimeSlot.Contains(timeSlot));
        }
    }
}