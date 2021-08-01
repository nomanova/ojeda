using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NomaNova.Ojeda.Api.Exceptions;
using NomaNova.Ojeda.Api.Models;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Core.Helpers.Interfaces;
using NomaNova.Ojeda.Models;

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
            private readonly IMapper _mapper;
            private readonly ISerializer _serializer;

            public ExceptionHandlerMiddleware(
                RequestDelegate next,
                ILogger<ExceptionHandlerMiddleware> logger,
                IMapper mapper,
                ISerializer serializer)
            {
                _next = next;
                _logger = logger;
                _mapper = mapper;
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
                    await BadRequestResponse(context, ex.Error);
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

            private async Task BadRequestResponse(HttpContext context, Error error)
            {
                await ErrorResponse(context, HttpStatusCode.BadRequest, error);
            }

            private async Task ErrorResponse(
                HttpContext context, HttpStatusCode statusCode, string message)
            {
                await ErrorResponse(context, statusCode, Error.General(message));
            }

            private async Task ErrorResponse(
                HttpContext context, HttpStatusCode statusCode, Error error)
            {
                var response = context.Response;
                
                response.StatusCode = (int) statusCode;
                response.ContentType = MediaTypeNames.Application.Json;

                var errorDto = _mapper.Map<ErrorDto>(error);
                var json = _serializer.Serialize(errorDto);

                await response.WriteAsync(json);
            }

            private async Task InternalServerErrorResponse(HttpContext context, Exception ex)
            {
                var correlationId = Guid.NewGuid();
                var message = $"Unhandled exception, correlation id: {correlationId}";
                
                _logger.LogCritical(ex, "{Message}", message);

                await ErrorResponse(context, HttpStatusCode.InternalServerError, message);
            }
        }
    }
}