using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts
{
    public class AccountAuthorization
    {
        /// <summary>
        /// Generate TokenValidationParameters
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters TokenValidationParameters =>
            new TokenValidationParameters
            {
                ValidIssuer = "ValidIssuer",
                ValidAudience = "ValidAudience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a8424887-7e2a-45e6-b59f-ae84caaf156d"))
            };
    }
}
