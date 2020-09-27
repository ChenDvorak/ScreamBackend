using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts
{
    internal class Formator
    {
        /// <summary>
        /// Normalized email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal static string EmailNormaliz(string email)
        {
            return email.ToUpper();
        }

        internal static string UsernameNormaliz(string username)
        {
            return username.ToUpper();
        }

        internal static string SplitUsernameFromEmail(string email)
        {
            var chars = email.Split('@');
            if (chars.Length < 2)
                throw new FormatException("Wrong email format");
            return chars[0];
        }
    }
}
