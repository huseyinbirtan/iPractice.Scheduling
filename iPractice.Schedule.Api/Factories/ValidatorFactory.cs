using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace iPractice.Scheduling.Api.Factories
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }

    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider ServiceProvider;
        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return ServiceProvider.GetService<IValidator<T>>();
        }
    }
}
