using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test
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
            Screams.IScreamsManager mockScreamsManager = new Screams.DefaultScreamsManager(_db, redisConn);

            var mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            { 
                Author = FakerUser,
                Content = "TEST: RIGTH COMMENT"
            };

            //  act
            var result = await mockScream.PostCommentAsync(fakerComment);

            //  assert
            Assert.True(result.Successed);
        }


        [Fact]
        public async void PostComment_EmptyContent_ReturnUnsuccessful()
        {
            //  arrange
            const int ERROR_COUNT = 1;
            const string ERROR = "评论内容不能为空";

            Screams.IScreamsManager mockScreamsManager = new Screams.DefaultScreamsManager(_db, redisConn);

            var mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            {
                Author = FakerUser,
                Content = ""
            };

            //  act
            var result = await mockScream.PostCommentAsync(fakerComment);

            //  assert
            Assert.False(result.Successed);
            Assert.Equal(result.Errors.Count, ERROR_COUNT);
            Assert.Equal(result.Errors.First(), ERROR);
        }

        [Fact]
        public async void PostComment_NullAuthor_ReturnUnsuccessful()
        {
            //  arrange

            Screams.IScreamsManager mockScreamsManager = new Screams.DefaultScreamsManager(_db, redisConn);
            Screams.ICommentsManager mockCommentsManager = new Screams.DefaultCommentsManager(_db);

            var mockScream = await mockScreamsManager.GetScreamAsync(1);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            {
                Author = null,
                Content = "TEST: RIGHT COMMENT"
            };

            //  act
            var t = await Assert.ThrowsAsync<NullReferenceException>(
                        async () => await mockScream.PostCommentAsync(fakerComment));
            //  assert
            Assert.NotNull(t);
        }
    }
}
