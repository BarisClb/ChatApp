using System.ComponentModel;

namespace ChatApp.Domain.Enums
{
    public enum EmailVerificationType
    {
        [Description("User Activation")]
        UserActivation = 1,
        [Description("Password Change")]
        PasswordChange = 2,
        [Description("Forgot Password")]
        ForgotPassword = 3
    }
}
