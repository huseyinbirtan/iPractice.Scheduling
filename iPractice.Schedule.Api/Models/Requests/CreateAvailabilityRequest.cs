using System;
using iPractice.Scheduling.Api.Models.Dtos;

namespace iPractice.Scheduling.Api.Models.Requests
{
    public class CreateAvailabilityRequest
    {
        public long PsychologistId { get; set; }
        public TimeSlotDto AvailabilityTimeSlot { get; set; }
    }
}