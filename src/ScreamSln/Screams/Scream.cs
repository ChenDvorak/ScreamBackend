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
        internal Scream(ScreamBackend.DB.Tables.Scream scream)
        {
            Model = scream;
            Cache_Key = CACHE_KEY_PREFIX + scream.Id;
        }

        /// <summary>
        /// valify screamid
        /// </summary>
        /// <param name="screamId"></param>
        /// <returns></returns>
        internal static bool IsValidId(int screamId) => screamId > 0;

        internal static string GetCacheKey(int screamId) => CACHE_KEY_PREFIX + screamId;

        /// <summary>
        /// parse scream database model to scream subject
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
