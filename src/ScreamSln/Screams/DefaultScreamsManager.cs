using Infrastructures;
using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public class DefaultScreamsManager : IScreamsManager
    {
        private const int LIST_CONTENT_LIMIT_LENGTH = 50;

        private readonly ScreamDB _db;
        public DefaultScreamsManager(ScreamDB db)
        {
            _db = db;
        }

        public async Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model)
        {
            var newScream = new ScreamBackend.DB.Tables.Scream
            { 
                Author = model.Author,
                Content = model.Content,
                CreateDate = DateTime.Now
            };
            await _db.Screams.AddAsync(newScream);
            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful(newScream.Id);
            throw new Exception("发布失败");
        }

        public Task<ScreamResult> RemoveAsync(Scream screamtion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get scream list with paging
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<Screams> GetScreamsAsync(int index, int size)
        {
            var screamsPaging = Screams.Create(index, size);

            screamsPaging.List = await _db.Screams.FromSqlRaw(BuildSQL())
                                           .AsNoTracking()
                                           .OrderByDescending(scream => scream.CreateDate)
                                           .Where(s => !s.Hidden)
                                           .Skip(screamsPaging.Skip)
                                           .Take(screamsPaging.Size)
                                           .Include(s => s.Author)
                                           .Select(s => new Screams.SingleItem
                                           { 
                                               Id = s.Id,
                                               AuthorId = s.AuthorId,
                                               Author = s.Author.UserName,
                                               Content = s.Content,
                                               DateTime = s.CreateDate.ToShortDateString()
                                           })
                                           .ToListAsync();
            screamsPaging.TotalSize = await _db.Screams.CountAsync(s => !s.Hidden);

            return screamsPaging;
        }

        private string BuildSQL()
        {
            return $@"SELECT 
Id, AuthorId, 
IF(CHAR_LENGTH(Content) > {LIST_CONTENT_LIMIT_LENGTH}, concat(left(content, {LIST_CONTENT_LIMIT_LENGTH}), '{LIST_CONTENT_LIMIT_LENGTH}'), Content) as Content,
HiddenCount, Hidden, AuditorId, CreateDate
FROM SCREAM";
        }

        public Task<Scream> GetScream(int screamId)
        {
            throw new NotImplementedException();
        }
    }
}
