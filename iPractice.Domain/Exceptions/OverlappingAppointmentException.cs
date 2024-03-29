using iPractice.SharedKernel.Exceptions;

namespace iPractice.Scheduling.Domain.Exceptions
{
    public class OverlappingAppointmentException : DomainLogicException
    {
        public OverlappingAppointmentException() : base("Psychologist is busy in the given timeslot")
        {
            
        }
    }
}
