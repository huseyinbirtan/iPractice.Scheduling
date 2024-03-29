using System;

namespace iPractice.Scheduling.Api.Models.Dtos
{
    public record TimeSlotDto(DateTime StartTime, DateTime EndTime);
}
