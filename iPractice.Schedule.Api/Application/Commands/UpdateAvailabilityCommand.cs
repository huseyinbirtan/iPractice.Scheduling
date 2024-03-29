using iPractice.Scheduling.Api.Models.Dtos;
using MediatR;
using System;

namespace iPractice.Scheduling.Api.Application.Commands
{
    public class UpdateAvailabilityCommand : INotification
    {
        public Guid AvailablityId { get; set; }
        public long PsychologistId { get; set; }
        public TimeSlotDto AvailabilityTimeSlot { get; set; }

        public UpdateAvailabilityCommand(Guid availablityId, long psychologistId, TimeSlotDto availabilityTimeSlot)
        {
            AvailablityId = availablityId;
            PsychologistId = psychologistId;
            AvailabilityTimeSlot = availabilityTimeSlot;
        }
    }
}
