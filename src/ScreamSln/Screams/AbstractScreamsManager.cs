using System.Threading.Tasks;

namespace Screams
{
    /// <summary>
    /// There is abstract class of scream
    /// </summary>
    public abstract class AbstractScreamsManager : IScreamsManager
    {
        internal readonly ScreamBackend.DB.ScreamDB DB;

        internal AbstractScreamsManager(ScreamBackend.DB.ScreamDB db)
        {
            DB = db;
        }

        public abstract Task<Scream> GetScreamAsync(int screamId);
        public abstract Task<ScreamPaging> GetScreamsAsync(int index, int size);
        public abstract Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model);
        public abstract Task<ScreamResult> RemoveAsync(int screamId);
    }
}
