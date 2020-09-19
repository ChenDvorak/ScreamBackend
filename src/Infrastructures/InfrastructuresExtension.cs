using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            RedisCache.Init(connectionString);
        }
    }
}
