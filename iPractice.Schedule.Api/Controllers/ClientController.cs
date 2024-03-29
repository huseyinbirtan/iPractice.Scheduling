using iPractice.Scheduling.Api.Application;
using iPractice.Scheduling.Api.Application.Commands;
using iPractice.Scheduling.Api.Models.Dtos;
using iPractice.Scheduling.Api.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IValidatorFactory = iPractice.Scheduling.Api.Factories.IValidatorFactory;

namespace iPractice.Scheduling.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : BaseController
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IReadModel ReadModel;

        public ClientController(ILogger<ClientController> logger, IReadModel readModel, IValidatorFactory validatorFactory, IMediator mediator) : base(validatorFactory, mediator)
        {
            _logger = logger;
            ReadModel = readModel;
        }

        /// <summary>
        /// The client can see his two psychologists.
        /// </summary>
        /// <param name="clientId">The client ID</param>
        /// <returns>All psychologist for the selected client</returns>
        [HttpGet("{clientId}/psychologists")]
        [ProducesResponseType(typeof(IEnumerable<PsychologistDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PsychologistDto>>> GetPsychologists(long clientId)
        {
            return Ok(await ReadModel.GetPsychologistsOfClient(clientId));
        }

        /// <summary>
        /// Create an appointment for a given availability slot
        /// </summary>
        /// <param name="clientId">The client ID</param>
        /// <param name="timeSlot">Identifies the client and availability slot</param>
        /// <returns>Ok if appointment was made</returns>
        [HttpPost("{clientId}/appointments")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAppointment(long clientId, [FromBody] CreateAppointmentRequest request)
        {
            request.ClientId = clientId;

            ValidateRequest(request);

            await Mediator.Publish(new CreateAppointmentCommand(request.ClientId, request.PsychologistId, request.TimeSlot));
            return Ok();
        }
    }
}
