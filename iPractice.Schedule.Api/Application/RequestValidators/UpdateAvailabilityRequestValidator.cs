using FluentValidation;
using iPractice.Scheduling.Api.Models.Requests;
using System.Linq;
using System;

namespace iPractice.Scheduling.Api.Applicaiton.RequestValidators
{
    public class UpdateAvailabilityRequestValidator : AbstractValidator<UpdateAvailabilityRequest>
    {
        public UpdateAvailabilityRequestValidator()
        {
            RuleFor(x => x.AvailablityId).NotEmpty();
            RuleFor(x => x.PsychologistId).NotEqual(0);
            RuleFor(x => x.AvailabilityTimeSlot).NotNull();
            RuleFor(x => x.AvailabilityTimeSlot.StartTime).GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be greater than current time.");
            RuleFor(x => x.AvailabilityTimeSlot.StartTime).LessThan(x => x.AvailabilityTimeSlot.EndTime).WithMessage("EndTime can never be earlier than StartTime");

            var allowedMinutes = new int[] { 0, 15, 30, 45 };
            RuleFor(x => x.AvailabilityTimeSlot.StartTime.Minute).Custom((min, context) => { if (!allowedMinutes.Contains(min)) { context.AddFailure("StartTime can only be in every quarter of hours ex. :00, :15, :30, :45"); } });
            RuleFor(x => x.AvailabilityTimeSlot.EndTime.Minute).Custom((min, context) => { if (!allowedMinutes.Contains(min)) { context.AddFailure("StartTime can only be in every quarter of hours ex. :00, :15, :30, :45"); } });
        }
    }
}
