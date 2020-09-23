using Infrastructures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using StackExchange.Redis;
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
        private readonly IDatabase _redis;
        public DefaultScreamsManager(ScreamDB db, ConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis.GetDatabase();
        }

        public async Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model)
        {
            const int NOT_DATA = 0;
            if (model.Author == null)
                return QuickResult.Unsuccessful(NOT_DATA, "作者不能为空");
            if (string.IsNullOrWhiteSpace(model.Content))
                return QuickResult.Unsuccessful(NOT_DATA, "内容不能为空");

            if (!await _db.Users.AnyAsync(user => user.Id == model.Author.Id))
                return QuickResult.Unsuccessful(NOT_DATA, "该作者不存在");

            var newScream = new ScreamBackend.DB.Tables.Scream
            {
                Author = model.Author,
                Content = model.Content,
                CreateDate = DateTime.Now,
                State = (int)Scream.Status.WaitAudit
            };
            await _db.Screams.AddAsync(newScream);
            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful(newScream.Id);
            throw new Exception("发布失败");
        }

        public async Task<ScreamResult> RemoveAsync(int screamId)
        {
            if (!Scream.IsValidId(screamId))
                throw new NullReferenceException("invalid scream Id");

            var scream = await _db.Screams.AsNoTracking().Where(s => s.Id == screamId).SingleOrDefaultAsync();
            var comments = await _db.Comments.AsNoTracking().Where(c => c.ScreamId == screamId).ToListAsync();
            _db.Comments.RemoveRange(comments);
            if (scream == null)
                throw new NullReferenceException("scream not exist");
            _db.Screams.Remove(scream);

            int effects = await _db.SaveChangesAsync();
            if (effects == comments.Count + 1)
            {
                await _redis.KeyDeleteAsync(Scream.GetCacheKey(screamId));
                return QuickResult.Successful();
            }
            return QuickResult.Unsuccessful("Delete scream fail");
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

            screamsPaging.List = await _db.Screams
#if RELEASE
                                           .FromSqlRaw(BuildSQL())
#endif
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
            const string CONTENT = nameof(ScreamBackend.DB.Tables.Scream.Content);

            return $@"SELECT 
                    {nameof(ScreamBackend.DB.Tables.Scream.Id)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.AuthorId)}, 
                    IF(CHAR_LENGTH({CONTENT}) > {LIST_CONTENT_LIMIT_LENGTH}, concat(left({CONTENT}, {LIST_CONTENT_LIMIT_LENGTH}), '{LIST_CONTENT_LIMIT_LENGTH}'), {CONTENT}) as {CONTENT},
                    {nameof(ScreamBackend.DB.Tables.Scream.HiddenCount)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.Hidden)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.AuditorId)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.CreateDate)}
                    FROM 
                    {nameof(ScreamDB.Screams)}";
        }

        /// <summary>
        /// get the subject of scream
        /// </summary>
        /// <param name="screamId"></param>
        /// <returns></returns>
        public async Task<Scream> GetScreamAsync(int screamId)
        {
            if (!Scream.IsValidId(screamId))
                return null;

            string redisValue;
            string currentKey = Scream.GetCacheKey(screamId);
            Scream result = null;

            if (await _redis.KeyExistsAsync(currentKey))
            {
                redisValue = await _redis.StringGetAsync(currentKey);
                result = new Scream(
                    JsonConvert.DeserializeObject<ScreamBackend.DB.Tables.Scream>(redisValue)
                );
            }
            else
            {
                var model = await _db.Screams.AsNoTracking().SingleOrDefaultAsync(s => s.Id == screamId);
                if (model == null)
                    return null;
                result = Scream.Parse(model);
                redisValue = JsonConvert.SerializeObject(result.Model);
                await _redis.StringSetAsync(key: currentKey, redisValue);
            }
            await _redis.KeyExpireAsync(currentKey, TimeSpan.FromHours(1));
            return result;
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
