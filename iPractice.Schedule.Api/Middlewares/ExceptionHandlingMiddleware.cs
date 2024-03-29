using FluentValidation;
using iPractice.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace iPractice.Scheduling.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly Dictionary<Type, HttpStatusCode> typeStatusCodeDictionary = new Dictionary<Type, HttpStatusCode> {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(DomainValidationException), HttpStatusCode.BadRequest },
            { typeof(DomainLogicException), HttpStatusCode.Conflict },
            { typeof(KeyNotFoundException), HttpStatusCode.NotFound }
        };

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private async Task<bool> SetContextByException(HttpContext context, Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            var type = ex.GetType();
            var statusCode = HttpStatusCode.OK;

            var hasFound = typeStatusCodeDictionary.TryGetValue(type, out statusCode) ? true : typeStatusCodeDictionary.TryGetValue(type.BaseType, out statusCode);

            if (hasFound == false)
            {
                return await SetContextByException(context, ex.InnerException);
            }
            else
            {
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(ex.Message);
                return true;
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //catch (ValidationException ex)
            //{
            //    await SetContextByException(context, ex);
            //}
            //catch (DomainValidationException ex)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    await context.Response.WriteAsync(ex.Message);
            //}
            //catch (DomainLogicException ex)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            //    await context.Response.WriteAsync(ex.Message);
            //}
            //catch (KeyNotFoundException ex)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            //    await context.Response.WriteAsync(ex.Message);
            //}
            catch (Exception exception)
            {
                var isKnownException = await SetContextByException(context, exception);

                if (isKnownException == false)
                {
                    _logger.LogError(
                    exception, "Exception occurred: {Message}", exception.Message);

                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server Error"
                    };

                    context.Response.StatusCode =
                        StatusCodes.Status500InternalServerError;

                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            }
        }
    }
}
