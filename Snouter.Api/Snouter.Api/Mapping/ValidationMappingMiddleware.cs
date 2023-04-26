﻿using FluentValidation;
using Snouter.Contracts.Responses;

namespace Snouter.Api.Mapping
{
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate _next;
        public ValidationMappingMiddleware(RequestDelegate next)
        {
            _next = next;   
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                httpContext.Response.StatusCode = 400;
                var validationFailureResponse = new ValidationFaliureResponse
                {
                    Errors = ex.Errors.Select(x => new ValidationResponse
                    {
                        PropertyName = x.PropertyName,
                        Message = x.ErrorMessage,
                    })
                };

                await httpContext.Response.WriteAsJsonAsync(validationFailureResponse);
            }
        }
    }
}
