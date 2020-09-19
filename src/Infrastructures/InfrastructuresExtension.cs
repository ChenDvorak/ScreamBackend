using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructures
{
    public static class InfrastructuresExtension
    {
        /// <summary>
        /// Redis Init
        /// </summary>
        /// <param name="services"></param>
        public static void RedisInit(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(ConnectionMultiplexer.Connect(connectionString));
        }
    }
}
