﻿using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    /// <summary>
    /// the subject of scream
    /// </summary>
    public class Scream
    {
        /// <summary>
        /// where it from
        /// </summary>
        private readonly AbstractScreamsManager _referenceScreams;
        private readonly ScreamBackend.DB.ScreamDB _db;

        internal ScreamBackend.DB.Tables.Scream Model { get; }

        private const string CACHE_KEY_PREFIX = "d3338d92-18d3-4e87-87fd-fbe79e2a6daa";
        internal readonly string Cache_Key;

        /// <summary>
        /// scream's state
        /// </summary>
        [Flags]
        public enum Status
        {
            WaitAudit = 0 << 1,
            Passed = 0 << 2,
            Recycle = 0 << 3,
        }

        /// <summary>
        /// instance from database model
        /// </summary>
        /// <param name="scream"></param>
        public Scream(ScreamBackend.DB.Tables.Scream scream)
        {
            Model = scream;
            Cache_Key = CACHE_KEY_PREFIX + scream.Id;
        }

        internal Scream(ScreamBackend.DB.Tables.Scream scream, AbstractScreamsManager referenceScreams) : this(scream)
        {
            _referenceScreams = referenceScreams;
            _db = referenceScreams.DB;
        }

        /// <summary>
        /// Scream full content
        /// </summary>
        public string FullContent
        {
            get
            {
                if (Model == null)
                    throw new NullReferenceException("scream can't null");
                return Model.Content;
            }
        }

        /// <summary>
        /// valify screamid
        /// </summary>
        /// <param name="screamId"></param>
        /// <returns></returns>
        internal static bool IsValidId(int screamId) => screamId > 0;

        internal static string GetCacheKey(int screamId) => CACHE_KEY_PREFIX + screamId;

        /// <summary>
        /// Post comment for scream
        /// </summary>
        /// <param name="scream"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<ScreamResult> PostCommentAsync(Models.NewComment comment)
        {
            if (comment.Author == null)
                throw new NullReferenceException("scream or model can't be null");
            if (string.IsNullOrWhiteSpace(comment.Content))
                return QuickResult.Unsuccessful("评论内容不能为空");
            if (comment.Content.Length < AbstractCommentsManager.COMMENT_MIN_LENGTH)
                return QuickResult.Unsuccessful($"评论内容必须大于{AbstractCommentsManager.COMMENT_MIN_LENGTH}个字");
            if (comment.Content.Length > AbstractCommentsManager.COMMENT_MAX_LENGTH)
                return QuickResult.Unsuccessful($"评论内容必须小于{AbstractCommentsManager.COMMENT_MAX_LENGTH}个字");

            var newComment = new ScreamBackend.DB.Tables.Comment
            {
                ScreamId = Model.Id,
                Content = comment.Content,
                AuthorId = comment.Author.Id,
                State = (int)Status.WaitAudit
            };

            await _referenceScreams.DB.Comments.AddAsync(newComment);

            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful();
            throw new Exception("post comment fail");
        }

        public async Task<ScreamResult> RemoveCommentAsync(int commentId)
        {
            if (!AbstractCommentsManager.IsValidCommentId(commentId))
                throw new ArgumentOutOfRangeException("comment Id is not a valid integer");
            var comment = await _db.Comments.AsNoTracking()
                                            .Where(c => c.Id == commentId)
                                            .SingleOrDefaultAsync();
            if (comment == null)
                return QuickResult.Unsuccessful("该评论不存在");

            _db.Comments.Remove(comment);
            int effected = await _db.SaveChangesAsync();
            if (effected == 1)
                return QuickResult.Successful();
            throw new Exception($"remove comment fail: Id: {commentId}");
        }

    }
}
