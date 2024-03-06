namespace ChatApp.Application.Helpers
{
    public static class LogHelper
    {
        public static string ErrorMessageTemplate = "{@Method} {@Request} {@ExceptionMessage}";
        public static string ExceptionMessageTemplate = "{@Method} {@Request} {@ExceptionMessage}";
        public static async Task<string> GetExceptionMessage(string languageCode)
        {
            return languageCode switch
            {
                "tr" => "Beklenmedik bir hata oluştu.",
                _ => "Something went wrong while processing your request."
            };
        }
    }
}
