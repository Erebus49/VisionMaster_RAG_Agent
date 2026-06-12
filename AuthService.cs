using System;

namespace ChatDemoCs
{
    public static class AuthService
    {
        private static string registeredUserName = "123";
        private static string registeredPassword = "123";

        public static void Register(string userName, string password)
        {
            registeredUserName = userName == null ? string.Empty : userName.Trim();
            registeredPassword = password == null ? string.Empty : password;
        }

        public static bool Validate(string userName, string password)
        {
            string name = userName == null ? string.Empty : userName.Trim();
            string pwd = password == null ? string.Empty : password;
            return string.Equals(name, registeredUserName, StringComparison.Ordinal) &&
                   string.Equals(pwd, registeredPassword, StringComparison.Ordinal);
        }
    }
}
