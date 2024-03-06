namespace ChatApp.Application.Models.Responses.Common
{
    public sealed class ApiResponse<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }


        public static ApiResponse<T> Success(int statusCode)
        {
            return new ApiResponse<T> { Data = default, StatusCode = statusCode, IsSuccess = true };
        }

        public static ApiResponse<T> Success(T data, int statusCode)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode, IsSuccess = true };
        }

        public static ApiResponse<T> Fail(string error, int statusCode)
        {
            return new ApiResponse<T> { Data = default, StatusCode = statusCode, IsSuccess = false, Errors = new List<string>() { error } };
        }

        public static ApiResponse<T> Fail(List<string> errors, int statusCode)
        {
            return new ApiResponse<T> { Data = default, StatusCode = statusCode, IsSuccess = false, Errors = errors };
        }
    }

    public class ApiResponseTokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
