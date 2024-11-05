using Core.Exceptions;
using Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace ToDoList.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ReturnModel<List<string>> Errors = new();

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;


            if (exception.GetType() == typeof(NotFoundException))
            {
                httpContext.Response.StatusCode = 404;
                Errors.Success = false;
                Errors.Message = exception.Message;
                Errors.StatusCode = 404;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

                return true;

            }

            if (exception.GetType() == typeof(BusinessException))
            {
                httpContext.Response.StatusCode = 400;
                Errors.Success = false;
                Errors.Message = exception.Message;
                Errors.StatusCode = 400;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

                return true;
            }

            if (exception.GetType() == typeof(UnauthorizedAccessException))
            {
                httpContext.Response.StatusCode = 401;
                Errors.Success = false;
                Errors.Message = exception.Message;
                Errors.StatusCode = 401;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

                return true;
            }

            if (exception.GetType() == typeof(ValidationException))
            {
                httpContext.Response.StatusCode = 400;
                Errors.Data = ((ValidationException)exception).Errors.Select(e => e.PropertyName).ToList();
                Errors.Success = false;
                Errors.Message = exception.Message;
                Errors.StatusCode = 400;
            }

            Errors.StatusCode = 500;
            Errors.Message = exception.Message;
            Errors.Success = false;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));
            return true;
        }
    }
    }