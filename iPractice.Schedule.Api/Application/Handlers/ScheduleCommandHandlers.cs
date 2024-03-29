using AutoMapper;
using iPractice.Scheduling.Api.Application.Commands;
using iPractice.Scheduling.Domain.Exceptions;
using iPractice.Scheduling.Domain.ScheduleAggregate;
using iPractice.Scheduling.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace iPractice.Scheduling.Api.Application.Handlers
{
    public class ScheduleCommandHandlers : INotificationHandler<CreateAppointmentCommand>, INotificationHandler<CreateAvailabilityCommand>, INotificationHandler<UpdateAvailabilityCommand>
    {
        private readonly IScheduleRepository ScheduleRepository;
        private readonly IMapper Mapper;

        public ScheduleCommandHandlers(
                IMapper mapper,
                IScheduleRepository scheduleRepository)
        {
            ScheduleRepository = scheduleRepository;
            Mapper = mapper;
        }

        private async Task<Schedule> GetScheduleByPsychologistId(long psychologistId)
        {
            var schedule = await ScheduleRepository.GetByPsychologistIdAsync(psychologistId);

            if (schedule == null)
            {
                throw new KeyNotFoundException("Either psychologist or schedule for provided psychologistId was not found");
            }

            return schedule;
        }

        public async Task Handle(CreateAppointmentCommand notification, CancellationToken cancellationToken)
        {
            var schedule = await GetScheduleByPsychologistId(notification.PsychologistId);

            var appointmentTimeSlot = Mapper.Map<TimeSlotValueObject>(notification.TimeSlot);

            var availability = schedule.GetAvailablityContainingTimeSlot(appointmentTimeSlot);

            if (availability == null)
            {
                throw new TimeSlotOutOfAvailablityException();
            }

            var appointment = new Appointment(Guid.NewGuid(), availability.Id, notification.ClientId, appointmentTimeSlot);

            availability.AddAppointment(appointment);

            await ScheduleRepository.SaveChangesAsync();
        }

        public async Task Handle(CreateAvailabilityCommand notification, CancellationToken cancellationToken)
        {
            var schedule = await ScheduleRepository.GetByPsychologistIdAsync(notification.PsychologistId);

            if (schedule == null)
            {
                schedule = new Schedule(Guid.NewGuid(), notification.PsychologistId);
                await ScheduleRepository.AddAsync(schedule);
            }

            var availablity = new Availability(Guid.NewGuid(), schedule.Id, notification.PsychologistId, Mapper.Map<AvailabilityTimeSlotValueObject>(notification.AvailabilityTimeSlot));

            schedule.CreateAvailability(availablity);

            await ScheduleRepository.SaveChangesAsync();
        }

        public async Task Handle(UpdateAvailabilityCommand notification, CancellationToken cancellationToken)
        {
            var schedule = await GetScheduleByPsychologistId(notification.PsychologistId);

            var availability = schedule.Availabilities.FirstOrDefault(a => a.Id == notification.AvailablityId);

            if (availability == null)
            {
                throw new KeyNotFoundException("Availablity for given Psychologist was not found");
            }

            availability.UpdateAvailablityTimeSlot(Mapper.Map<AvailabilityTimeSlotValueObject>(notification.AvailabilityTimeSlot));

            await ScheduleRepository.SaveChangesAsync();
        }
    }
}
