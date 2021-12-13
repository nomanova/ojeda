using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Api.Middleware
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }

        public class ExceptionHandlerMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionHandlerMiddleware> _logger;
            private readonly ISerializer _serializer;

            public ExceptionHandlerMiddleware(
                RequestDelegate next,
                ILogger<ExceptionHandlerMiddleware> logger,
                ISerializer serializer)
            {
                _next = next;
                _logger = logger;
                _serializer = serializer;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next.Invoke(context);
                }
                catch (NotFoundException)
                {
                    NotFoundResponse(context);
                }
                catch (ValidationException ex)
                {
                    await BadRequestResponse(context, ex.ValidationErrors);
                }
                catch (ForbiddenException)
                {
                    ForbiddenResponse(context);
                }
                catch (Exception ex)
                {
                    await InternalServerErrorResponse(context, ex);
                }
            }

            private static void NotFoundResponse(HttpContext context)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            private static void ForbiddenResponse(HttpContext context)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            }

            private async Task BadRequestResponse(HttpContext context, Dictionary<string, List<string>> validationErrors)
            {
                var errorDto = new ErrorDto
                {
                    Code = (int) HttpStatusCode.BadRequest,
                    Message = "Invalid request",
                    ValidationErrors = validationErrors
                };

                var json = _serializer.Serialize(errorDto);

                await ErrorResponse(context, HttpStatusCode.BadRequest, json);
            }

            private async Task InternalServerErrorResponse(HttpContext context, Exception ex)
            {
                var correlationId = Guid.NewGuid();
                _logger.LogCritical(ex, "Correlation id: {CorrelationId}", correlationId);

                var errorDto = new ErrorDto
                {
                    Code = (int) HttpStatusCode.InternalServerError,
                    Message = $"Correlation id: {correlationId}"
                };

                var json = _serializer.Serialize(errorDto);

                await ErrorResponse(context, HttpStatusCode.InternalServerError, json);
            }

            private static async Task ErrorResponse(HttpContext context, HttpStatusCode statusCode, string json)
            {
                var response = context.Response;

                response.StatusCode = (int) statusCode;
                response.ContentType = MediaTypeNames.Application.Json;

                await response.WriteAsync(json);
            }
        }
    }
}