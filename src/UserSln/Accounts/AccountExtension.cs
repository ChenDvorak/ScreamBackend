using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Accounts.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Accounts
{
    public static class AccountExtension
    {
        public static void UseAccount(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager<UserManager>, UserManager>();
            services.AddScoped<IAccountManager<AdminManager>, AdminManager>();

            services.AddSingleton<IAuthorizationHandler, IsAdministratorHandler>();

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(IsAdministratorPolicy.POLICY, policy => policy.Requirements.Add(new IsAdministratorRequirement()));
            });
        }
    }
}
