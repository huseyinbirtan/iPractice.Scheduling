using iPractice.Scheduling.Api.Models.Dtos;
using System.Collections.Generic;

namespace iPractice.Scheduling.Api.Models.Responses
{
    public class GetPsychologistsResponse
    {
        public List<PsychologistDto> Psychologists { get; set; }
    }
}
