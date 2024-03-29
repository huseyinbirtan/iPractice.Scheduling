using iPractice.Scheduling.Api.Application.IntegrationEvents.Events;
using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace iPractice.Scheduling.Api.Application.IntegrationEvents.Handlers
{
    public class PsychologistCreatedEventHandler : INotificationHandler<PsychologistCreatedEvent>
    {
        private readonly IRepository<Psychologist> PsychologistRepository;
        private readonly ILogger<PsychologistCreatedEventHandler> Logger;
        public PsychologistCreatedEventHandler(IRepository<Psychologist> psychologistRepository, ILogger<PsychologistCreatedEventHandler> logger)
        {
            PsychologistRepository = psychologistRepository;
            Logger = logger;
        }

        public async Task Handle(PsychologistCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await PsychologistRepository.AddAsync(notification.Psychologist);

                await PsychologistRepository.SaveChangesAsync();

                //TODO unit of work can be implemented to create both Psychologist and Schedule here
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "An error occured while creating psychologist.");
            }
        }
    }
}
