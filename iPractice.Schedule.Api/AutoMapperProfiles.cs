using AutoMapper;
using iPractice.Scheduling.Api.Models.Dtos;
using iPractice.Scheduling.Domain.SyncAggregates;
using iPractice.Scheduling.Domain.ValueObjects;

namespace iPractice.Scheduling.Api
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TimeSlotDto, TimeSlotValueObject>().ReverseMap();
            CreateMap<TimeSlotDto, AvailabilityTimeSlotValueObject>().ReverseMap();
            CreateMap<Psychologist, PsychologistDto>();
        }
    }
}
