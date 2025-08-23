using GithubCommitsToMusic.Exceptions;
using System.Net;
using System.Text.Json;

namespace GithubCommitsToMusic.Middlewares
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException exception)
            {
                // log the error
                _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();
                await response.WriteAsync(exception.Message);
            }
            catch (Exception exception)
            {
                // log the error
                _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
                var response = context.Response;
                response.ContentType = "application/json";

                // get the response code and message
                response.StatusCode = 400;
                var json = JsonSerializer.Serialize(new { Message = exception.Message });
                await response.WriteAsync(json);
            }
        }
    }
}
