using iPractice.SharedKernel.Exceptions;

namespace iPractice.Scheduling.Domain.Exceptions
{
    public class UnavailablePsychologistException : DomainLogicException
    {
        public UnavailablePsychologistException() : base("Psychologist is not available in the given timeslot")
        {
            
        }
    }
}
