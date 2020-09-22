using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Screams
{
    /// <summary>
    /// the subject of scream
    /// </summary>
    public class Scream
    {
        internal ScreamBackend.DB.Tables.Scream Model { get; }

        private const string CACHE_KEY_PREFIX = "d3338d92-18d3-4e87-87fd-fbe79e2a6daa";
        internal readonly string Cache_Key;

        /// <summary>
        /// scream's state
        /// </summary>
        [Flags]
        public enum Status
        {
            WaitAudit = 0 << 1,
            Passed = 0 << 2,
            Recycle = 0 << 3,
        }

        /// <summary>
        /// instance from database model
        /// </summary>
        /// <param name="scream"></param>
        internal Scream(ScreamBackend.DB.Tables.Scream scream)
        {
            Model = scream;
            Cache_Key = CACHE_KEY_PREFIX + scream.Id;
        }
        ///// <summary>
        ///// instance from redis
        ///// </summary>
        ///// <param name="cacheKey"></param>
        //internal Scream(string cacheKey, IDatabase redis)
        //{
        //    const string MODEL_KEY = "MODEL";
        //    Cache_Key = cacheKey;
            
        //}

        /// <summary>
        /// valify screamid
        /// </summary>
        /// <param name="screamId"></param>
        /// <returns></returns>
        internal static bool IsValidId(int screamId) => screamId > 0;

        internal static string GetCacheKey(int screamId) => CACHE_KEY_PREFIX + screamId;

        /// <summary>
        /// parse scream database model to scream subject
        /// will set cache
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static Scream Parse(ScreamBackend.DB.Tables.Scream model)
        {
            if (model == null)
                throw new NullReferenceException();

            return new Scream(model);
        }
    }
}
