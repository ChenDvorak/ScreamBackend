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

namespace Screams.Screams
{
    public class DefaultScreamsManager : AbstractScreamsManager
    {
        /// <summary>
        /// The content will cut the longer which in list if content length longer than this value
        /// </summary>
        private const int LIST_CONTENT_LIMIT_LENGTH = 50;

        private readonly IDatabase _redis;
        public DefaultScreamsManager(ScreamDB db, ConnectionMultiplexer redis) : base(db)
        {
            _redis = redis.GetDatabase();
        }

        public override async Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model)
        {
            const int NOT_DATA = 0;
            if (model.Author == null)
                return QuickResult.Unsuccessful(NOT_DATA, "作者不能为空");
            if (string.IsNullOrWhiteSpace(model.Content))
                return QuickResult.Unsuccessful(NOT_DATA, "内容不能为空");

            if (!await DB.Users.AnyAsync(user => user.Id == model.Author.Id))
                return QuickResult.Unsuccessful(NOT_DATA, "该作者不存在");

            var newScream = new ScreamBackend.DB.Tables.Scream
            {
                AuthorId = model.Author.Id,
                Content = model.Content,
                ContentLength = model.Content.Length,
                CreateDate = DateTime.Now,
                State = (int)Scream.Status.WaitAudit
            };
            await DB.Screams.AddAsync(newScream);
            int effects = await DB.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful(newScream.Id);
            throw new Exception("发布失败");
        }

        public override async Task<ScreamResult> RemoveAsync(int screamId)
        {
            if (!Scream.IsValidId(screamId))
                throw new NullReferenceException("invalid scream Id");

            var scream = await DB.Screams.AsNoTracking().Where(s => s.Id == screamId).SingleOrDefaultAsync();
            var comments = await DB.Comments.AsNoTracking().Where(c => c.ScreamId == screamId).ToListAsync();
            DB.Comments.RemoveRange(comments);
            if (scream == null)
                throw new NullReferenceException("scream not exist");
            DB.Screams.Remove(scream);

            int effects = await DB.SaveChangesAsync();
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
        public override async Task<ScreamPaging> GetScreamsAsync(int index, int size)
        {
            var screamsPaging = ScreamPaging.Create(index, size);

            screamsPaging.List = await DB.Screams
                                           .FromSqlRaw(BuildSQL())
                                           .AsNoTracking()
                                           .OrderByDescending(scream => scream.CreateDate)
                                           .Where(s => !s.Hidden)
                                           .Skip(screamsPaging.Skip)
                                           .Take(screamsPaging.Size)
                                           .Include(s => s.Author)
                                           .Select(s => new ScreamPaging.SingleItem
                                           {
                                               Id = s.Id,
                                               AuthorId = s.AuthorId,
                                               Author = s.Author.Username,
                                               Content = s.ContentLength > LIST_CONTENT_LIMIT_LENGTH
                                                        ? string.Concat(s.Content, "...")
                                                        : s.Content,
                                               IsFullContent = s.ContentLength <= LIST_CONTENT_LIMIT_LENGTH,
                                               HiddenCount = s.HiddenCount,
                                               DateTime = s.CreateDate.ToShortDateString()
                                           })
                                           .ToListAsync();
            screamsPaging.TotalSize = await DB.Screams.CountAsync(s => !s.Hidden);

            return screamsPaging;
        }

        private string BuildSQL()
        {
            const string CONTENT = nameof(ScreamBackend.DB.Tables.Scream.Content);
            //  IF(CHAR_LENGTH({CONTENT}) > {LIST_CONTENT_LIMIT_LENGTH}, concat(left({CONTENT}, {LIST_CONTENT_LIMIT_LENGTH}), '{LIST_CONTENT_LIMIT_LENGTH}'), {CONTENT}) as {CONTENT},

            var sql = $@"SELECT 
                    {nameof(ScreamBackend.DB.Tables.Scream.Id)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.AuthorId)}, 
                    SUBSTR({CONTENT}, 1, {LIST_CONTENT_LIMIT_LENGTH})  as {CONTENT},
                    {nameof(ScreamBackend.DB.Tables.Scream.ContentLength)},
                    {nameof(ScreamBackend.DB.Tables.Scream.HiddenCount)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.Hidden)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.AuditorId)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.State)}, 
                    {nameof(ScreamBackend.DB.Tables.Scream.CreateDate)}
                    FROM 
                    {nameof(ScreamDB.Screams)}";
            return sql;
        }

        /// <summary>
        /// get the subject of scream
        /// </summary>
        /// <param name="screamId"></param>
        /// <returns></returns>
        public override async Task<Scream> GetScreamAsync(int screamId)
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
                    JsonConvert.DeserializeObject<ScreamBackend.DB.Tables.Scream>(redisValue),
                    this
                );
            }
            else
            {
                var model = await DB.Screams.AsNoTracking().SingleOrDefaultAsync(s => s.Id == screamId);
                if (model == null)
                    return null;
                result = new Scream(model, this);
                redisValue = JsonConvert.SerializeObject(result.Model);
                await _redis.StringSetAsync(key: currentKey, redisValue);
            }
            await _redis.KeyExpireAsync(currentKey, TimeSpan.FromHours(1));
            return result;
        }
    }
}
