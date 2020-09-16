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

        public async Task<ScreamPaging> GetScreams(Infrastructures.Paging<ScreamPaging.ScreamItem> paging)
        {
            throw new NotImplementedException();
        }
    }
}
