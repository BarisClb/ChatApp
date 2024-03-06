namespace ChatApp.Application.Models.Exceptions
{
    public class ApiException : Exception
    {
        public string? PublicErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
        public string? Method { get; set; }
        public Exception? OriginalException { get; set; }
        public bool LogException { get; set; } = true;
        public string? OverrideLogMessage { get; set; }
    }
}
