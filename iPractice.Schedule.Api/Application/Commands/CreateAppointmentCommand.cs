using iPractice.Scheduling.Api.Models.Dtos;
using MediatR;

namespace iPractice.Scheduling.Api.Application.Commands
{
    public record CreateAppointmentCommand(long ClientId, long PsychologistId, TimeSlotDto TimeSlot) : INotification;
}
