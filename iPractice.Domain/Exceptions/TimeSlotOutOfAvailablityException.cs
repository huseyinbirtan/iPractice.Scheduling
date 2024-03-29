using iPractice.SharedKernel.Exceptions;

namespace iPractice.Scheduling.Domain.Exceptions
{
    public class TimeSlotOutOfAvailablityException : DomainLogicException
    {
        public TimeSlotOutOfAvailablityException() : base("Given timeslot is not in availability")    
        {
        }

        public TimeSlotOutOfAvailablityException(string message) : base (message)
        {
            
        }
    }
}
