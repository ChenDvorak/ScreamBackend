using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public class DefaultCommentsManager : ICommentsManager
    {
        /// <summary>
        /// 评论内容最大长度
        /// </summary>
        internal const int COMMENT_MAX_LENGTH = 200;
        /// <summary>
        /// 评论内容最小长度
        /// </summary>
        internal const int COMMENT_MIN_LENGTH = 4;

        private readonly ScreamDB _db;
        public DefaultCommentsManager(ScreamDB db)
        {
            _db = db;
        }

        /// <summary>
        /// Post comment for scream
        /// </summary>
        /// <param name="scream"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<ScreamResult> PostCommentAsync(Models.NewComment comment)
        {
            if (comment.Scream == null || comment.Scream.Model == null)
                throw new NullReferenceException("scream or model can't be null");
            if (comment.Author == null)
                throw new NullReferenceException("scream or model can't be null");
            if (string.IsNullOrWhiteSpace(comment.Content))
                return QuickResult.Unsuccessful("评论内容不能为空");
            if (comment.Content.Length < COMMENT_MIN_LENGTH)
                return QuickResult.Unsuccessful($"评论内容必须大于{COMMENT_MIN_LENGTH}个字");
            if (comment.Content.Length > COMMENT_MAX_LENGTH)
                return QuickResult.Unsuccessful($"评论内容必须小于{COMMENT_MAX_LENGTH}个字");

            var newComment = new Comment
            {
                ScreamId = comment.Scream.Model.Id,
                Content = comment.Content,
                AuthorId = comment.Author.Id,
                State = (int)Scream.Status.WaitAudit
            };

            await _db.Comments.AddAsync(newComment);

            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful();
            throw new Exception("post comment fail");
        }
    }
}
