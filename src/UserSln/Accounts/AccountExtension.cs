using Microsoft.Extensions.DependencyInjection;

namespace Accounts
{
    public static class AccountExtension
    {
        public static void UseAccount(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager<UserManager>, UserManager>();
        }
    }
}
