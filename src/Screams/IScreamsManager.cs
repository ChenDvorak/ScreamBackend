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
        /// get scream by screamId
        /// </summary>
        /// <param name="streamId"></param>
        /// <returns></returns>
        Task<Screamtion> GetScream(int screamId);
        /// <summary>
        /// get scream list of paging
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<ScreamResult<ScreamPaging>> GetScreamsAsync(Infrastructures.Paging<ScreamPaging.ScreamItem> paging);
        /// <summary>
        /// post a new scream
        /// </summary>
        /// <param name="model">the parameter what post need</param>
        /// <returns>the result that post</returns>
        Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model);
        /// <summary>
        /// remove a scream
        /// </summary>
        /// <param name="screamtion"></param>
        /// <returns></returns>
        Task<ScreamResult> RemoveAsync(Screamtion screamtion);
    }
}
