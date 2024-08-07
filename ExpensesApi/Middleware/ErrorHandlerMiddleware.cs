﻿
using ExpensesApi.Exceptions;
using ExpensesApi.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ExpensesApi.Middleware
{
    /// <summary>
    /// Middleware for handling errors in http request
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlerMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware for handle http context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context,error);
            }
        }

        /// <summary>
        /// Handles error status codes 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var apiResponse = new ApiResponse<string>
            {
                Success = false,
                Message = error.Message
            };

            _logger.LogError(error, "An unexpected error occurred.");

            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = error switch
            {
                ApiExceptions => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            response.StatusCode = statusCode;

            apiResponse.Message = JsonSerializer.Serialize(new { message = error?.Message });

            await response.WriteAsJsonAsync(apiResponse);
        }
    }
}
