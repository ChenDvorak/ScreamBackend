﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams.Screams
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
        Task<Scream> GetScreamAsync(int screamId);
        /// <summary>
        /// get scream list of paging
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<ScreamPaging> GetScreamsAsync(int index, int size);
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
        Task<ScreamResult> RemoveAsync(int screamId);
    }
}
