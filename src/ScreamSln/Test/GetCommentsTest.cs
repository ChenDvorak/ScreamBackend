using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    public class GetCommentsTest : DBSeedFactory
    {

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 10)]
        public async void GetComments_RightIndexAndSize_ReturnPaging(int index, int size)
        {
            //  arrange
            const int SCREAM_ID = 5;
            const int EXPECT_TOTAL_SIZE = 25;
            const int EXPECT_TOTAL_PAGE = 3;

            CreateComments(SCREAM_ID);

            Screams.ICommentsManager commentsManager = new Screams.DefaultCommentsManager(_db);
            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);
            //  act
            var paging = await commentsManager.GetCommentsAsync(scream, index, size);

            //  assert
            Assert.Equal(index, paging.Index);
            Assert.Equal(size, paging.Size);
            Assert.Equal(EXPECT_TOTAL_SIZE, paging.TotalSize);
            Assert.Equal(EXPECT_TOTAL_PAGE, paging.TotalPage);
            Assert.Equal(size, paging.List.Count);
        }

        private void CreateComments(int screamId)
        {
            const int COUNT = 25;
            List<Comment> fakeComments = new List<Comment>(COUNT);
            for (int i = 1; i <= COUNT; i++)
            {
                fakeComments.Add(new Comment
                {
                    Content = "TEST: FAKER COMMENT " + i,
                    AuthorId = FakerUser.Id,
                    ScreamId = screamId
                });
            }

            _db.Comments.AddRange(fakeComments);
            int t = _db.SaveChanges();
            if (t == COUNT)
                return;
            throw new Exception("added comments fail");
        }
    }
}
