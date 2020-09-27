using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreamBackend.ServitesExtension
{
    public static class AuthorizationExtension
    {

        public static IServiceCollection UseJWT(this IServiceCollection services, TokenValidationParameters tokenValidationParameters)
        {
            //  JWT 验证规则
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JWTOption =>
            {
                JWTOption.TokenValidationParameters = tokenValidationParameters;
            });
            return services;
        }
    }
}
