using ChatApp.Application.Commands.User;
using ChatApp.Application.Helpers;
using ChatApp.Application.Resources;
using ChatApp.Infrastructure.FluentValidation.Common;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ChatApp.Infrastructure.FluentValidation
{
    public class UserValidations
    {
        public class UserRegisterRequestValidator : BaseAbstractValidator<UserRegisterCommand>
        {
            public UserRegisterRequestValidator(IStringLocalizer<LocalizationResources> localizer)
            {
                RuleFor(request => request.FirstName).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.FirstName]));
                RuleFor(request => request.LastName).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.LastName]));
                RuleFor(request => request.EmailAddress).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.EmailAddress]))
                                                                  .EmailAddress().WithMessage(localizer[LocalizationKeys.EmailAddressFormatError]);
                RuleFor(request => request.Username).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.Username]))
                                                              .Length(5, 20).WithMessage(string.Format(localizer[LocalizationKeys.UsernameLengthError], 5, 20));
                RuleFor(request => request.Password).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.Password]))
                                                              .Length(5, 20).WithMessage(string.Format(localizer[LocalizationKeys.PasswordLengthError], 5, 20));
                RuleFor(request => request.LanguageCode).NotEmpty().WithMessage(string.Format(localizer[LocalizationKeys.FolowingFieldCannotBeEmpty], localizer[LocalizationKeys.Language]));
            }
        }
    }
}
