using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public interface ICommentsManager
    {
        /// <summary>
        /// Post comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<ScreamResult> PostComment(Models.NewComment comment);
    }
}
