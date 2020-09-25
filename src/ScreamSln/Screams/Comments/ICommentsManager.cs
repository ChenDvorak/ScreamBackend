using System.Threading.Tasks;

namespace Screams.Comments
{
    public interface ICommentsManager
    {
        /// <summary>
        /// Get comments with paging of scream
        /// </summary>
        /// <param name="scream"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        Task<CommentPaging> GetCommentsAsync(Screams.Scream scream, int index, int size);
        /// <summary>
        /// Get comment by comment Id
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment> GetCommentAsync(int commentId);
    }
}
