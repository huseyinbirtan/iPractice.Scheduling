using iPractice.Scheduling.Domain.Exceptions;
using iPractice.SharedKernel.Exceptions;

namespace iPractice.Scheduling.Domain.ValueObjects
{
    public class TimeSlotValueObject
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TimeSlotValueObject(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            if (endTime < startTime)
            {
                throw new DomainValidationException("EndTime can never be earlier than StartTime");
            }
        }

        public bool Overlaps(TimeSlotValueObject timeSlot)
        {
            return (timeSlot.StartTime >= this.StartTime && timeSlot.StartTime < this.EndTime) 
                || (timeSlot.EndTime > this.StartTime && timeSlot.EndTime <= this.EndTime)
                || (timeSlot.StartTime < this.StartTime && timeSlot.EndTime > this.EndTime);
        }
    }
}
