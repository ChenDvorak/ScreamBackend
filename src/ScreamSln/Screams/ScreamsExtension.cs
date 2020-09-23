using Microsoft.Extensions.DependencyInjection;

namespace Screams
{
    public static class ScreamsExtension
    {
        /// <summary>
        /// add services of scream and comment manager
        /// </summary>
        public static void UseScreamManager(this IServiceCollection services)
        {
            services.AddScoped<IScreamsManager, DefaultScreamsManager>();
            services.AddScoped<ICommentsManager, DefaultCommentsManager>();
        }
    }
}
