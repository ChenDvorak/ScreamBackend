using System.Threading.Tasks;

namespace Screams
{
    public interface ICommentsManager
    {
        /// <summary>
        /// Get comments with paging of scream
        /// </summary>
        /// <param name="scream"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        Task<CommentPaging> GetCommentsAsync(Scream scream, int index, int size);
    }
}
