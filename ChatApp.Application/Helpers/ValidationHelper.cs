using System.Text.RegularExpressions;

namespace ChatApp.Application.Helpers
{
    public static class ValidationHelper
    {
        public static bool CheckIfStringIsInEmailFormat(string str)
        {
            Regex emailRegex = new(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return emailRegex.Match(str).Success;
        }
    }
}
