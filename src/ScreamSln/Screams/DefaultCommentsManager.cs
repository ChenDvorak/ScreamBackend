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
        public async Task<ScreamResult> PostComment(Models.NewComment comment)
        {
            if (comment.Scream == null || comment.Scream.Model == null)
                throw new NullReferenceException("scream or model can't be null");
            if (comment.Author == null)
                throw new NullReferenceException("scream or model can't be null");
            if (string.IsNullOrWhiteSpace(comment.Content))
                return QuickResult.Unsuccessful("评论内容不能为空");

            _db.Comments.Add(new Comment
            {
                ScreamId = comment.Scream.Model.Id,
                Content = comment.Content,
                Author = comment.Author,
                State = (int)Scream.Status.WaitAudit
            });

            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful();
            throw new Exception("post comment fail");
        }
    }
}
