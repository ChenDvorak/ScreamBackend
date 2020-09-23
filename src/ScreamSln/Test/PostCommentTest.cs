using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test
{
    public class PostCommentTest : DBSeedFactory
    {
        /// <summary>
        /// Testing for post comment
        /// Return Successed
        /// </summary>
        [Fact]
        public async void PostComment_RightComment_ReturnSuccessful()
        {
            //  arrange
            Screams.IScreamsManager mockScreamsManager = new Screams.DefaultScreamsManager(_db, redisConn);
            Screams.ICommentsManager mockCommentsManager = new Screams.DefaultCommentsManager(_db);

            var scream = await mockScreamsManager.GetScreamAsync(1);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            { 
                Author = FakerUser,
                Scream = scream,
                Content = "TEST: RIGTH COMMENT"
            };

            //  act
            var result = await mockCommentsManager.PostCommentAsync(fakerComment);

            //  assert
            Assert.True(result.Successed);
        }


        [Fact]
        public async void PostComment_EmptyContent_ReturnUNsuccessful()
        {
            //  arrange
            const int ERROR_COUNT = 1;
            const string ERROR = "评论内容不能为空";

            Screams.IScreamsManager mockScreamsManager = new Screams.DefaultScreamsManager(_db, redisConn);
            Screams.ICommentsManager mockCommentsManager = new Screams.DefaultCommentsManager(_db);

            var scream = await mockScreamsManager.GetScreamAsync(1);

            Screams.Models.NewComment fakerComment = new Screams.Models.NewComment
            {
                Author = FakerUser,
                Scream = scream,
                Content = ""
            };

            //  act
            var result = await mockCommentsManager.PostCommentAsync(fakerComment);

            //  assert
            Assert.False(result.Successed);
            Assert.Equal(result.Errors.Count, ERROR_COUNT);
            Assert.Equal(result.Errors.First(), ERROR);
        }
    }
}
