using System.Net;
using TaskTracker.Core.Exceptions;
using TaskTracker.Core.Extensions;

namespace TaskTracker.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _logger.SendError($"Validation error in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = "application/json";
                
                var validationErrors = ex.ValidationResult.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                }).ToList();
                
                var errorResponse = new
                {
                    error = ex.ErrorCode,
                    message = "Doğrulama hatası",
                    details = validationErrors,
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (NotFoundException ex)
            {
                _logger.SendError($"Not found error in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    error = ex.ErrorCode,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (UnauthorizedException ex)
            {
                _logger.SendError($"Unauthorized error in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    error = ex.ErrorCode,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (BadRequestException ex)
            {
                _logger.SendError($"Bad request error in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    error = ex.ErrorCode,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (BaseException ex)
            {
                _logger.SendError($"Custom exception in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    error = ex.ErrorCode,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.SendError($"Unhandled exception in {httpContext.Request.Method} {httpContext.Request.Path}", ex);
                
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    error = "INTERNAL_SERVER_ERROR",
                    message = "Beklenmeyen bir hata oluştu",
                    timestamp = DateTime.UtcNow,
                    path = httpContext.Request.Path,
                    method = httpContext.Request.Method
                };
                
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
} 