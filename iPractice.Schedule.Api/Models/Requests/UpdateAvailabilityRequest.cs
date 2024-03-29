using System;
using iPractice.Scheduling.Api.Models.Dtos;

namespace iPractice.Scheduling.Api.Models.Requests
{
    public class UpdateAvailabilityRequest
    {
        public Guid AvailablityId { get; set; }
        public long PsychologistId { get; set; }
        public TimeSlotDto AvailabilityTimeSlot { get; set; }
    }
}