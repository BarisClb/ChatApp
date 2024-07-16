using System.Net;
using System.Text;

namespace ChatApp.WebHub.Configurations
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
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
            //string request = await GetRequestBody(context.Request);
            string method = "ErrorHandlerMiddleware";
            bool logException = true;
            string overrideLogMessage = default;

            switch (exception)
            {
                //case ApiException ex:
                //    logException = ex.LogException;
                //    if (ex.ErrorCode != null)
                //        httpCode = ex.ErrorCode ?? (int)HttpStatusCode.InternalServerError;
                //    if (!string.IsNullOrEmpty(ex.PublicErrorMessage))
                //        errors.Add(ex.PublicErrorMessage);
                //    if (!string.IsNullOrEmpty(ex.Method))
                //        method = ex.Method;
                //    if (ex.OriginalException != null)
                //        exception = ex.OriginalException;
                //    if (!string.IsNullOrEmpty(ex.OverrideLogMessage))
                //        overrideLogMessage = ex.OverrideLogMessage;
                //    break;
                //case ApiValidationException ex:
                //    logException = ex.LogException;
                //    httpCode = (int)HttpStatusCode.BadRequest;
                //    errors.AddRange(ex.Errors);
                //    break;
                //case AuthorizationException ex:
                //    logException = ex.LogException;
                //    httpCode = (int)HttpStatusCode.Unauthorized;
                //    if (!string.IsNullOrEmpty(ex.PublicErrorMessage))
                //        errors.Add(ex.PublicErrorMessage);
                //    break;
                default:
                    break;
            }

            //if (logException)
            //    Log.Error(exception, LogHelper.ExceptionMessageTemplate, method, request, overrideLogMessage ?? exception.Message);

            //if (!errors.Any())
            //    errors.Add(await LogHelper.GetExceptionMessage(context.Request.GetHeaderValue("Accept-Language") ?? ""));

            context.Response.ContentType = "application/json";
            //await context.Response.WriteAsync(JsonSerialize.SerializeObject(ApiResponse<NoContent>.Fail(errors, httpCode)));
            await context.Response.WriteAsync("FAIL");
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
                return "";
            }
        }
    }
}
