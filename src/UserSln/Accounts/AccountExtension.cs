using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts
{
    public static class AccountExtension
    {
        public static void UseAccount(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager, UserManager>();
        }
    }
}
