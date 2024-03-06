using FluentValidation.Results;

namespace ChatApp.Application.Models.Exceptions
{
    public class ApiValidationException : Exception
    {
        public IList<string> Errors { get; set; } = new List<string>();
        public bool LogError { get; set; }

        public ApiValidationException(List<ValidationFailure> failures)
        {
            foreach (var failure in failures)
                Errors.Add($"{failure.ErrorMessage}");
        }
    }
}
