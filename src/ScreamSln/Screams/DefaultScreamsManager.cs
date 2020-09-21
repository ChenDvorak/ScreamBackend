﻿using Infrastructures;
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

        public async Task<ScreamResult> RemoveAsync(Scream scream)
        {
            if (scream == null || scream.Model == null)
                throw new NullReferenceException("scream can't be null");

            var comments = await _db.Comments.AsNoTracking().Where(c => c.ScreamId == scream.Model.Id).ToListAsync();
            _db.Comments.RemoveRange(comments);
            _db.Screams.Remove(scream.Model);

            int effects = await _db.SaveChangesAsync();
            if (effects == comments.Count + 1)
            {
                await _redis.KeyDeleteAsync(scream.Cache_Key);
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
        public async Task<Scream> GetScream(int screamId)
        {
            if (!Scream.IsValidId(screamId))
                return null;

            string redisValue;
            string currentKey = Scream.GetCacheKey(screamId);
            Scream result = null;
            
            if (await _redis.KeyExistsAsync(currentKey))
            {
                redisValue = await _redis.StringGetAsync(currentKey);
                result = JsonConvert.DeserializeObject<Scream>(redisValue);
            }
            else
            {
                var model = await _db.Screams.AsNoTracking().SingleOrDefaultAsync(s => s.Id == screamId);
                if (model == null)
                    return null;
                result = Scream.Parse(model);
                redisValue = JsonConvert.SerializeObject(result);
                await _redis.StringSetAsync(key: currentKey, redisValue);
            }
            await _redis.KeyExpireAsync(currentKey, TimeSpan.FromHours(1));
            return result;
        }
    }
}
