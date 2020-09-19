using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Infrastructures
{
    public class RedisCache
    {
        private static ConnectionMultiplexer _conn;
        
        private RedisCache() 
        { }
        /// <summary>
        /// Initails Redis Connection
        /// should be invoke at ConfigureServices
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static void Init(string connectionString)
        {
            _conn = ConnectionMultiplexer.Connect(connectionString);
        }

        public static IDatabase GetRedas(int db = 0)
        {
            return _conn.GetDatabase(db);
        }
    }
}
