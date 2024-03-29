using iPractice.Scheduling.Api.Models.Dtos;
using MediatR;

namespace iPractice.Scheduling.Api.Application.Commands
{
    public class CreateAvailabilityCommand : INotification
    {
        public long PsychologistId { get; set; }
        public TimeSlotDto AvailabilityTimeSlot { get; set; }

        public CreateAvailabilityCommand(long psychologistId, TimeSlotDto availabilityTimeSlot)
        {
            PsychologistId = psychologistId;
            AvailabilityTimeSlot = availabilityTimeSlot;
        }
    }
}
