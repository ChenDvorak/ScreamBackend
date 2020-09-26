using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Screams.Comments
{
    public class DefaultCommentsManager : AbstractCommentsManager
    {
        
        public DefaultCommentsManager(ScreamDB db): base(db)
        { }

        /// <summary>
        /// Return Comment or null if not exist
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public override async Task<Comment> GetCommentAsync(int commentId)
        {
            if (!IsValidCommentId(commentId))
                throw new ArgumentOutOfRangeException("Invalid comment Id");
            var comment = await _db.Comments.AsNoTracking().SingleOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
                return null;
            var result = new Comment(comment, _db);
            return result;
        }

        public override async Task<CommentPaging> GetCommentsAsync(Screams.Scream scream, int index, int size)
        {
            if (scream == null || scream.Model == null)
                throw new NullReferenceException("scream of model can't be null");
            var paging = CommentPaging.Create(index, size);

            Expression<Func<ScreamBackend.DB.Tables.Comment, bool>> whereStatement =
                comment => comment.ScreamId == scream.Model.Id && !comment.Hidden;

            paging.List = await _db.Comments.AsNoTracking()
                                            .OrderByDescending(c => c.CreateDate)
                                            .Where(whereStatement)
                                            .Skip(paging.Skip)
                                            .Take(paging.Size)
                                            .Include(c => c.Author)
                                            .Select(c => new CommentPaging.Comment
                                            {
                                                Id = c.Id,
                                                AuthorId = c.AuthorId,
                                                Author = c.Author.Username,
                                                Content = c.Content,
                                                HiddenCount = c.HiddenCount,
                                                DateTime = c.CreateDate.ToShortDateString()
                                            })
                                            .ToListAsync();
            paging.TotalSize = await _db.Comments.CountAsync(whereStatement);
            return paging;
        }   
    }
}
