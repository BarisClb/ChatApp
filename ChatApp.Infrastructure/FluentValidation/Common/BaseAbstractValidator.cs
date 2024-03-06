using ChatApp.Application.Models.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace ChatApp.Infrastructure.FluentValidation.Common
{
    public abstract class BaseAbstractValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            ValidationResult validationResult = base.Validate(context);

            if (!validationResult.IsValid)
                throw new ApiValidationException(validationResult.Errors);

            return validationResult;
        }
    }
}
