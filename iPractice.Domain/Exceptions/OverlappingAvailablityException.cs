using iPractice.SharedKernel.Exceptions;

namespace iPractice.Scheduling.Domain.Exceptions
{
    public class OverlappingAvailablityException : DomainLogicException
    {
        public OverlappingAvailablityException() : base("There is an availability saved for the psychologist")
        {
            
        }
    }
}
