using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Application.Helpers
{
    public static class SecurityHelper
    {
        public static string GetSha256Hash(string input)
        {
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = SHA256.HashData(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
