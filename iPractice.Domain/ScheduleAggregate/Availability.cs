using iPractice.Scheduling.Domain.Events;
using iPractice.Scheduling.Domain.Exceptions;
using iPractice.Scheduling.Domain.ValueObjects;
using iPractice.SharedKernel.BaseClasses;

namespace iPractice.Scheduling.Domain.ScheduleAggregate
{
    public class Availability : BaseEntity<Guid>
    {
        private long psychologistId { get; set; }
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; private set; }
        public AvailabilityTimeSlotValueObject AvailabilityTimeSlot { get; private set; }

        private readonly List<Appointment> appointments = new List<Appointment>();
        public IEnumerable<Appointment> Appointments => appointments.AsReadOnly();

        private Availability() { }

        public Availability(Guid id, Guid scheduleId, long psychologistId, AvailabilityTimeSlotValueObject availabilityTimeSlot)
        {
            Id = id == Guid.Empty ? throw FieldException(nameof(id)) : id;
            ScheduleId = scheduleId == Guid.Empty ? throw FieldException(nameof(scheduleId)) : scheduleId;
            AvailabilityTimeSlot = availabilityTimeSlot == null ? throw FieldException(nameof(availabilityTimeSlot)) : availabilityTimeSlot;
            this.psychologistId = psychologistId;
        }

        public Availability AddAppointment(Appointment appointment)
        {
            if (!AvailabilityTimeSlot.Contains(appointment.TimeSlot))
            {
                throw new TimeSlotOutOfAvailablityException();
            }

            if (appointments.Any(a => a.TimeSlot.Overlaps(appointment.TimeSlot)))
            {
                throw new OverlappingAppointmentException();
            }

            appointments.Add(appointment);
            Events.Add(new AppointmentScheduledEvent(appointment.ClientId, psychologistId, appointment.TimeSlot.StartTime, appointment.TimeSlot.EndTime));
            return this; //For test practices
        }

        //TODO Test it
        public Availability UpdateAvailablityTimeSlot(AvailabilityTimeSlotValueObject availabilityTimeSlot)
        {
            if (appointments.Any(a => availabilityTimeSlot.Contains(a.TimeSlot) == false))
            {
                throw new TimeSlotOutOfAvailablityException("There are scheduled appointments in the availability can not be canceled");
            }

            AvailabilityTimeSlot = availabilityTimeSlot;
            return this; //For test practices
        }

        //TODO Test it
        public IEnumerable<TimeSlotValueObject> GetAvailableTimeSlots()
        {
            var availableTimeSlots = new List<TimeSlotValueObject>();
            var availabilityStart = AvailabilityTimeSlot.StartTime;

            if (appointments.Count > 0)
            {
                foreach (var appointment in appointments.OrderBy(a => a.TimeSlot.StartTime))
                {
                    var timeSlotStart = availabilityStart;
                    var timeSlotEnd = appointment.TimeSlot.StartTime;

                    if (timeSlotEnd >= timeSlotStart.AddMinutes(30))
                    {
                        availableTimeSlots.AddRange(SplitAvailabilities(timeSlotStart, timeSlotEnd));
                    }

                    availabilityStart = appointment.TimeSlot.EndTime;
                }
            }

            availableTimeSlots.AddRange(SplitAvailabilities(availabilityStart, AvailabilityTimeSlot.EndTime));

            return availableTimeSlots;
        }

        private IEnumerable<TimeSlotValueObject> SplitAvailabilities(DateTime start, DateTime end)
        {
            while (end >= start.AddMinutes(30))
            {
                var timeSlotEnd = start.AddMinutes(30);
                yield return new TimeSlotValueObject(start, timeSlotEnd);
                start = timeSlotEnd;
            }
        }
    }
}
