using Screams;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    public class RemoveCommentTest : DBSeedFactory
    {
        const int SCREAM_ID = 6;


        public RemoveCommentTest(): base()
        {
            CreateComments(SCREAM_ID);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async void RemoveComment_ExistCommentId_ReturnSuccessful(int commentId)
        {
            //  arrange
            
            IScreamsManager mockScreamsManager = new DefaultScreamsManager(_db, redisConn);
            Scream mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            var result = await mockScream.RemoveCommentAsync(commentId);

            //  assert
            Assert.True(result.Successed);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void RemoveComment_InvalidCommentId_ThrowException(int commentId)
        {
            //  arrange
            IScreamsManager mockScreamsManager = new DefaultScreamsManager(_db, redisConn);
            Scream mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            var thr = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    async () => await mockScream.RemoveCommentAsync(commentId));

            //  assert
            Assert.NotNull(thr);
        }

        private void CreateComments(int screamId)
        {
            const int COUNT = 25;
            List<ScreamBackend.DB.Tables.Comment> fakeComments = new List<ScreamBackend.DB.Tables.Comment>(COUNT);
            for (int i = 1; i <= COUNT; i++)
            {
                fakeComments.Add(new ScreamBackend.DB.Tables.Comment
                {
                    Content = "TEST: FAKER COMMENT " + i,
                    AuthorId = FakerUser.Id,
                    ScreamId = screamId
                });
            }

            using var context = new ScreamBackend.DB.ScreamDB(contextOptions);

            context.Comments.AddRange(fakeComments);
            int t = context.SaveChanges();
            if (t == COUNT)
                return;
            throw new Exception("added comments fail");
        }
    }
}
