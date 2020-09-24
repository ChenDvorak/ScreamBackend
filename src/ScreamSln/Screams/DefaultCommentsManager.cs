using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public class DefaultCommentsManager : AbstractCommentsManager
    {
        
        public DefaultCommentsManager(ScreamDB db): base(db)
        { }

        public override async Task<CommentPaging> GetCommentsAsync(Scream scream, int index, int size)
        {
            if (scream == null || scream.Model == null)
                throw new NullReferenceException("scream of model can't be null");
            var paging = CommentPaging.Create(index, size);

            Expression<Func<Comment, bool>> whereStatement =
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
                                                Author = c.Author.UserName,
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
