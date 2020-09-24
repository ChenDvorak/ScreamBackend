using ScreamBackend.DB;
using System;
using System.Threading.Tasks;

namespace Screams.Comments
{
    public abstract class AbstractCommentsManager : ICommentsManager
    {
        /// <summary>
        /// 评论内容最大长度
        /// </summary>
        internal const int COMMENT_MAX_LENGTH = 200;
        /// <summary>
        /// 评论内容最小长度
        /// </summary>
        internal const int COMMENT_MIN_LENGTH = 4;

        protected readonly ScreamDB _db;

        protected AbstractCommentsManager(ScreamDB db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public abstract Task<CommentPaging> GetCommentsAsync(Screams.Scream scream, int index, int size);
        /// <summary>
        /// Validate comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public static bool IsValidCommentId(int commentId) => commentId > 0;

    }
}
