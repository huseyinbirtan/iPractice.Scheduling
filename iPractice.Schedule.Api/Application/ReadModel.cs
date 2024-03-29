using AutoMapper;
using iPractice.Scheduling.Api.Models.Dtos;
using iPractice.Scheduling.Domain.ScheduleAggregate;
using iPractice.Scheduling.Domain.SyncAggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iPractice.Scheduling.Api.Application
{
    public interface IReadModel
    {
        Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlotsOfPsychologist(long psychologistId);
        Task<IEnumerable<PsychologistDto>> GetPsychologistsOfClient(long clientId);
    }

    public class ReadModel : IReadModel
    {
        private readonly IScheduleRepository ScheduleRepository;
        private readonly IClientReadRepository ClientReadRepository;
        private readonly IMapper Mapper;
        public ReadModel(IScheduleRepository scheduleRepository, IClientReadRepository clientReadRepository, IMapper mapper)
        {
            ScheduleRepository = scheduleRepository;
            ClientReadRepository = clientReadRepository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlotsOfPsychologist(long psychologistId)
        {
            var schedule = await ScheduleRepository.GetByPsychologistIdAsync(psychologistId);

            if (schedule == null)
            {
                throw new KeyNotFoundException("Either psychologist or schedule for provided psychologistId was not found");
            }

            return Mapper.Map<List<TimeSlotDto>>(schedule.GetAvailableTimeSlots());
        }

        public async Task<IEnumerable<PsychologistDto>> GetPsychologistsOfClient(long clientId)
        {
            var client = await ClientReadRepository.GetClientWithPsychologists(clientId);

            if (client == null)
            {
                throw new KeyNotFoundException("Client was not found");
            }

            return Mapper.Map<List<PsychologistDto>>(client.Psychologists);
        }
    }
}
