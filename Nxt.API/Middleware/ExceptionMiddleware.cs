using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Nxt.Common.Exceptions;

namespace Nxt.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _next = next;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, _webHostEnvironment);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment webHostEnvironment)
        {
            if (context.Response.HasStarted)
                throw exception;

            var reference = exception.InnerException is NxtException ex ? ex.ReferenceId.ToString() : (exception is NxtException) ? (exception as NxtException).ReferenceId.ToString() : "unavailable";

            var exceptionSummary = new ExceptionSummary
            {
                UserMessage = "There was an error processing your request.",
                SupportMessages = $"There reference for this exception is {reference} ",
                ReferenceId = reference
            };

            var errorStatusCode = StatusCodes.Status500InternalServerError;
            var appExcetion = exception as NxtException;
            if (appExcetion != null)
            {
                switch (appExcetion.ExceptionCode)
                {
                    case ExceptionCodes.Validation:
                        errorStatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case ExceptionCodes.Restriction:
                    case ExceptionCodes.UnAuthorized:
                        errorStatusCode = StatusCodes.Status403Forbidden;
                        break;
                    case ExceptionCodes.ItemNotFound:
                        errorStatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        break;
                }
            }

            while (exception != null)
            {
                var exceptionCode = (exception as NxtException)?.ExceptionCode;
                if (exceptionCode == ExceptionCodes.Validation ||
                    exceptionCode == ExceptionCodes.Restriction ||
                    exceptionCode == ExceptionCodes.Operation ||
                    exceptionCode == ExceptionCodes.ItemNotFound ||
                    exceptionCode == ExceptionCodes.UnAuthorized)
                {
                    exceptionSummary.ValidationMessage = $"{exception.Message}";
                    break;
                }

                if (webHostEnvironment.IsDevelopment())
                    exceptionSummary.SupportMessages = $"{Environment.NewLine}{exception.Message}";

                exception = exception.InnerException;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorStatusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionSummary));
        }
    }
}
