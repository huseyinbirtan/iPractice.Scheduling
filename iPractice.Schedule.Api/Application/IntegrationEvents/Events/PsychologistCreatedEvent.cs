using iPractice.Scheduling.Domain.SyncAggregates;
using MediatR;

namespace iPractice.Scheduling.Api.Application.IntegrationEvents.Events
{
    public record PsychologistCreatedEvent(Psychologist Psychologist) : INotification { }
}
