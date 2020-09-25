using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using Screams.Comments;
using Screams.Screams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test
{
    public class IncreaseCommentHidden : DBSeedFactory
    {
        [Fact]
        public async void Increase_NotInput_RetrunIncreasedHiddenCount()
        {
            //  arrange
            int commentId = CreateComment();
            int expected = ExpectedHiddenCount(commentId) + 1;
            int actual;
            ICommentsManager commentsManager = new DefaultCommentsManager(_db);
            var comment = await commentsManager.GetCommentAsync(commentId);

            //  act
            actual = await comment.IncreaseHidden();
            //  assert
            Assert.Equal(expected, actual);
        }

        private int CreateComment()
        {
            using var context = new ScreamDB(contextOptions);

            ScreamBackend.DB.Tables.Scream fakeScream = new ScreamBackend.DB.Tables.Scream
            { 
                Content = "TEST: FAKE SCREAM",
                ContentLength = 17,
                AuthorId = FakeUser.Id,
            };

            ScreamBackend.DB.Tables.Comment fakeComment = new ScreamBackend.DB.Tables.Comment
            { 
                Content = "TEST: FAKE COMMENT",
                AuthorId = FakeUser.Id,
                Scream = fakeScream
            };
            context.Screams.Add(fakeScream);
            context.Comments.Add(fakeComment);
            context.SaveChanges();
            return fakeComment.Id;
        }

        private int ExpectedHiddenCount(int commentId)
        {
            using var context = new ScreamDB(contextOptions);
            var comment = context.Comments.AsNoTracking().SingleOrDefault(s => s.Id == commentId);
            if (comment == null)
                throw new NullReferenceException();
            return comment.HiddenCount;
        }
    }
}
