using Microsoft.Extensions.DependencyInjection;
using Screams.Comments;

namespace Screams
{
    public static class ScreamsExtension
    {
        /// <summary>
        /// add services of scream and comment manager
        /// </summary>
        public static void UseScreamManager(this IServiceCollection services)
        {
            services.AddScoped<Screams.IScreamsManager, Screams.DefaultScreamsManager>();
            services.AddScoped<ICommentsManager, DefaultCommentsManager>();
        }
    }
}
