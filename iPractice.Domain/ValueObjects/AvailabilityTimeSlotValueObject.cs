namespace iPractice.Scheduling.Domain.ValueObjects
{
    public class AvailabilityTimeSlotValueObject : TimeSlotValueObject
    {
        public AvailabilityTimeSlotValueObject(DateTime startTime, DateTime endTime) : base(startTime,endTime)
        {
        }

        public bool Contains(TimeSlotValueObject timeSlot)
        {
            return StartTime <= timeSlot.StartTime && EndTime >= timeSlot.EndTime;
        }
    }
}
