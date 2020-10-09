using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Accounts
{
    public class AccountAuthorization
    {
        public static readonly string Issuer = "https://scream.com";
        public static readonly string Audience = "ValidAudience";
        public static readonly byte[] SigningKey = Encoding.UTF8.GetBytes("a8424887-7e2a-45e6-b59f-ae84caaf156d");
        public static readonly int ExpireMinutes = 44640;
        /// <summary>
        /// Generate TokenValidationParameters
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a8424887-7e2a-45e6-b59f-ae84caaf156d"))
        };

        public static JwtSecurityToken GetJwtSecurityToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(SigningKey);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken
                (
                    Issuer,
                    Audience,
                    claims,
                    notBefore: null,
                    DateTime.UtcNow.AddMinutes(ExpireMinutes),
                    credentials
                );
        }
    }
}
