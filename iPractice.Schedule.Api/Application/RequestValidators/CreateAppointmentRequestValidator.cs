using FluentValidation;
using iPractice.Scheduling.Api.Models.Requests;
using System;
using System.Linq;

namespace iPractice.Scheduling.Api.Applicaiton.RequestValidators
{
    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEqual(0);
            RuleFor(x => x.PsychologistId).NotEqual(0);
            RuleFor(x => x.TimeSlot).NotNull();
            RuleFor(x => x.TimeSlot.StartTime).GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be greater than current time.");
            RuleFor(x => x.TimeSlot.StartTime).LessThan(x => x.TimeSlot.EndTime).WithMessage("EndTime can never be earlier than StartTime");

            var allowedMinutes = new int[] { 0, 15, 30, 45 };
            RuleFor(x => x.TimeSlot.StartTime.Minute).Custom((min, context) => { if (!allowedMinutes.Contains(min)) { context.AddFailure("StartTime can only be in every quarter of hours ex. :00, :15, :30, :45"); } });
            RuleFor(x => x.TimeSlot.EndTime.Minute).Custom((min, context) => { if (!allowedMinutes.Contains(min)) { context.AddFailure("StartTime can only be in every quarter of hours ex. :00, :15, :30, :45"); } });
        }
    }
}
