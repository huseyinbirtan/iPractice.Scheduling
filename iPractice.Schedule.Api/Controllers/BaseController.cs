using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IValidatorFactory = iPractice.Scheduling.Api.Factories.IValidatorFactory;

namespace iPractice.Scheduling.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IValidatorFactory ValidatorFactory;
        protected readonly IMediator Mediator;

        public BaseController(IValidatorFactory validatorFactory, IMediator mediator)
        {
            ValidatorFactory = validatorFactory;
            Mediator = mediator;
        }

        protected void ValidateRequest<T>(T request) where T : new()
        {
            var validator = ValidatorFactory.GetValidator<T>();
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid == false)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
