using ChatApp.Application.Helpers;
using ChatApp.Application.Models.Exceptions;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;

namespace ChatApp.Application.Configurations.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public ErrorHandlerMiddleware(RequestDelegate next, IStringLocalizer<LocalizationResources> localization)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception exception)
            {
                await ExceptionHandler(context, exception);
            }
        }

        private async Task ExceptionHandler(HttpContext context, Exception exception)
        {
            int httpCode = (int)HttpStatusCode.InternalServerError;
            List<string> errors = new();
            string request = await GetRequestBody(context.Request);
            string method = "ErrorHandlerMiddleware";
            bool logException = true;
            string overrideLogMessage = default;

            switch (exception)
            {
                case ApiException ex:
                    if (ex.ErrorCode != null)
                        httpCode = ex.ErrorCode ?? (int)HttpStatusCode.InternalServerError;
                    if (!string.IsNullOrEmpty(ex.PublicErrorMessage))
                        errors.Add(ex.PublicErrorMessage);
                    if (!string.IsNullOrEmpty(ex.Method))
                        method = ex.Method;
                    if (ex.OriginalException != null)
                        exception = ex.OriginalException;
                    if (!string.IsNullOrEmpty(ex.OverrideLogMessage))
                        overrideLogMessage = ex.OverrideLogMessage;
                    logException = ex.LogException;
                    break;
                case ApiValidationException ex:
                    logException = ex.LogError;
                    httpCode = (int)HttpStatusCode.BadRequest;
                    errors.AddRange(ex.Errors);
                    break;
                default:
                    break;
            }

            if (logException)
                Log.Error(exception, LogHelper.ExceptionMessageTemplate, method, request, overrideLogMessage ?? exception.Message);

            if (!errors.Any())
                errors.Add(await LogHelper.GetExceptionMessage(context.Request.GetHeaderValue("Accept-Language") ?? ""));

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(ApiResponse<NoContent>.Fail(errors, httpCode)));
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            try
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using StreamReader reader = new StreamReader(request.Body, Encoding.UTF8);
                return await reader.ReadToEndAsync();
            }
            catch (Exception exception)
            {
                Log.Error(exception, LogHelper.ExceptionMessageTemplate, "ExceptionHandler.GetRequestBody", null, exception.Message);
                return "";
            }
        }
    }
}
