namespace ChatApp.Application.Models.Exceptions
{
    public class AuthorizationException : Exception
    {
        public string? PublicErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
        public string? Method { get; set; }
        public Exception? OriginalException { get; set; }
        public bool LogException { get; set; } = false;

        public AuthorizationException(string? publicErrorMessage = default, int? errorCode = default, string? method = null, Exception? originalException = null, bool logException = false)
        {
            PublicErrorMessage = publicErrorMessage;
            ErrorCode = errorCode;
            Method = method;
            OriginalException = originalException;
            LogException = logException;
        }
    }
}
