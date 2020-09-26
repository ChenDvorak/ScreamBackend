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
    }
}
