using Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public class DefaultScreamsManager : IScreamsManager
    {
        public Task<ScreamPaging> GetScreamsAsync(Paging<ScreamPaging.ScreamItem> paging)
        {
            throw new NotImplementedException();
        }

        public Task<ScreamResult<ScreamPaging.ScreamItem>> PostScreamAsync(Models.NewScreamtion model)
        {
            throw new NotImplementedException();
        }

        public Task<ScreamResult> RemoveAsync(Screamtion screamtion)
        {
            throw new NotImplementedException();
        }
    }
}
