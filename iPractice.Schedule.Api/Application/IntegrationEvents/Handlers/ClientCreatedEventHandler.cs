using iPractice.Scheduling.Api.Application.IntegrationEvents.Events;
using iPractice.Scheduling.Api.Controllers;
using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.SharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace iPractice.Scheduling.Api.Application.IntegrationEvents.Handlers
{
    public class ClientCreatedEventHandler : INotificationHandler<ClientCreatedEvent>
    {
        private readonly IRepository<Client> ClientRepository;
        private readonly ILogger<ClientCreatedEventHandler> Logger;
        public ClientCreatedEventHandler(IRepository<Client> clientRepository, ILogger<ClientCreatedEventHandler> logger)
        {
            ClientRepository = clientRepository;
            Logger = logger;
        }

        public async Task Handle(ClientCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await ClientRepository.AddAsync(notification.Client);
                await ClientRepository.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "An error occured while creating client.");
            }
        }
    }
}
