using iPractice.Scheduling.Api.Application;
using iPractice.Scheduling.Api.Application.Commands;
using iPractice.Scheduling.Api.Models.Dtos;
using iPractice.Scheduling.Api.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IValidatorFactory = iPractice.Scheduling.Api.Factories.IValidatorFactory;

namespace iPractice.Scheduling.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PsychologistController : BaseController
    {
        private readonly ILogger<PsychologistController> _logger;
        private readonly IReadModel ReadModel;


        public PsychologistController(ILogger<PsychologistController> logger, IReadModel readModel, IValidatorFactory validatorFactory, IMediator mediator) : base(validatorFactory, mediator)
        {
            _logger = logger;
            ReadModel = readModel;
        }

        [HttpGet]
        public string Get()
        {
            return "Success!";
        }

        /// <summary>
        /// To get available timeslots of the given Psychologist
        /// </summary>
        /// <param name="psychologistId"></param>
        /// <returns>Available time slots</returns>
        [HttpGet("{psychologistId}/availabileTimeSlots")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetAvailableTimeSlots([FromRoute] long psychologistId)
        {
            return Ok(await ReadModel.GetAvailableTimeSlotsOfPsychologist(psychologistId));
        }

        /// <summary>
        /// Add a block of time during which the psychologist is available during normal business hours
        /// </summary>
        /// <param name="psychologistId"></param>
        /// <param name="availability">Availability</param>
        /// <returns>Ok if the availability was created</returns>
        [HttpPost("{psychologistId}/availability")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAvailability([FromRoute] long psychologistId, [FromBody] CreateAvailabilityRequest request)
        {
            request.PsychologistId = psychologistId;
            
            ValidateRequest(request);

            await Mediator.Publish(new CreateAvailabilityCommand(request.PsychologistId, request.AvailabilityTimeSlot));
            return Ok();
        }

        /// <summary>
        /// Update availability of a psychologist
        /// </summary>
        /// <param name="psychologistId">The psychologist's ID</param>
        /// <param name="availabilityId">The ID of the availability block</param>
        /// <returns>List of availability slots</returns>
        [HttpPut("{psychologistId}/availability/{availabilityId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAvailability([FromRoute] long psychologistId, [FromRoute] Guid availabilityId, [FromBody] UpdateAvailabilityRequest request)
        {
            request.PsychologistId = psychologistId;
            request.AvailablityId = availabilityId;

            ValidateRequest(request);

            await Mediator.Publish(new UpdateAvailabilityCommand(request.AvailablityId, request.PsychologistId, request.AvailabilityTimeSlot));
            return Ok();
        }
    }
}
