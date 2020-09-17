using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    /// <summary>
    /// interface of screams manager
    /// </summary>
    public interface IScreamsManager
    {
        /// <summary>
        /// get scream list of paging
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<ScreamResult<ScreamPaging>> GetScreamsAsync(Infrastructures.Paging<ScreamPaging.ScreamItem> paging);
        /// <summary>
        /// post a new scream
        /// </summary>
        /// <param name="model">the parameter what post need</param>
        /// <returns>the result that post</returns>
        public Task<ScreamResult<ScreamPaging.ScreamItem>> PostScreamAsync(Models.NewScreamtion model);
        /// <summary>
        /// remove a scream
        /// </summary>
        /// <param name="screamtion"></param>
        /// <returns></returns>
        public Task<ScreamResult> RemoveAsync(Screamtion screamtion);
    }
}
