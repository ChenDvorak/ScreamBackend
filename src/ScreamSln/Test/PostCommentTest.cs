﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Screams.Comments;
using System.Threading.Tasks;

namespace ScreamsTest
{
    public class PostCommentTest : DBSeedFactory
    {
        const int SCREAM_ID = 1;

        /// <summary>
        /// Testing for post comment
        /// Return Successed
        /// </summary>
        [Fact]
        public async void PostComment_RightComment_ReturnSuccessful()
        {
            //  arrange
            Screams.Screams.IScreamsManager mockScreamsManager = new Screams.Screams.DefaultScreamsManager(_db, redisConn);

            var mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            { 
                Author = FakeUser,
                Content = "TEST: RIGTH COMMENT"
            };

            //  act
            var result = await mockScream.PostCommentAsync(fakerComment);

            //  assert
            Assert.True(result.Succeeded);
        }


        [Fact]
        public async void PostComment_EmptyContent_ReturnUnsuccessful()
        {
            //  arrange
            const int ERROR_COUNT = 1;
            const string ERROR = "评论内容不能为空";

            Screams.Screams.IScreamsManager mockScreamsManager = new Screams.Screams.DefaultScreamsManager(_db, redisConn);

            var mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            {
                Author = FakeUser,
                Content = ""
            };

            //  act
            var result = await mockScream.PostCommentAsync(fakerComment);

            //  assert
            Assert.False(result.Succeeded);
            Assert.Equal(result.Errors.Count, ERROR_COUNT);
            Assert.Equal(result.Errors.First(), ERROR);
        }

        [Fact]
        public async void PostComment_NullAuthor_ReturnUnsuccessful()
        {
            //  arrange

            Screams.Screams.IScreamsManager mockScreamsManager = new Screams.Screams.DefaultScreamsManager(_db, redisConn);
            ICommentsManager mockCommentsManager = new DefaultCommentsManager(_db);

            var mockScream = await mockScreamsManager.GetScreamAsync(1);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            {
                Author = null,
                Content = "TEST: RIGHT COMMENT"
            };

            //  act
            Task actual() => mockScream.PostCommentAsync(fakerComment);

            //  assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }
    }
}
