using iPractice.Scheduling.Api.Models.Dtos;

namespace iPractice.Scheduling.Api.Models.Requests
{
    public class CreateAppointmentRequest
    {
        public long ClientId { get; set; }
        public long PsychologistId { get; set; }
        public TimeSlotDto TimeSlot { get; set; }
    }
}
