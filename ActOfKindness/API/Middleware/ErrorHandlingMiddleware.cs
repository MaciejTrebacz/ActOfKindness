using System.Net;
using Application.Exceptions;
using Serilog;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                Log.Warning($"User Id = {notFoundException.UserId} | Role = {notFoundException.UserRole} | [{notFoundException.Method}] | Action = {notFoundException.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(string.IsNullOrEmpty(notFoundException.UserMessage) ? notFoundException.Message : notFoundException.UserMessage);
            }
            catch (BadRequestException badRequestException)
            {
                Log.Error($"User Id = {badRequestException.UserId} | Role = {badRequestException.UserRole} | [{badRequestException.Method}] | Action = {badRequestException.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(string.IsNullOrEmpty(badRequestException.UserMessage) ? badRequestException.Message : badRequestException.UserMessage);
            }
            catch (ForbidException forbidException)
            {
                Log.Warning($"User Id = {forbidException.UserId} | Role = {forbidException.UserRole} | [{forbidException.Method}] | Action = {forbidException.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(string.IsNullOrEmpty(forbidException.UserMessage) ? forbidException.Message : forbidException.UserMessage);
            }
            catch (Exception exception)
            {
                Log.Fatal($"Exception {exception}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync($"Something went wrong");
            }
        }
    }
}
