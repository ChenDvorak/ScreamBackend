﻿using Screams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScreamsTest
{
    public class RemoveCommentTest : DBSeedFactory
    {
        const int SCREAM_ID = 6;


        public RemoveCommentTest() : base()
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

            Screams.Screams.IScreamsManager mockScreamsManager = new Screams.Screams.DefaultScreamsManager(_db, redisConn);
            Screams.Screams.Scream mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            var result = await mockScream.RemoveCommentAsync(commentId);

            //  assert
            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async void RemoveComment_InvalidCommentId_ThrowException(int commentId)
        {
            //  arrange
            Screams.Screams.IScreamsManager mockScreamsManager = new Screams.Screams.DefaultScreamsManager(_db, redisConn);
            Screams.Screams.Scream mockScream = await mockScreamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            Task actual() => mockScream.RemoveCommentAsync(commentId);

            //  assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(actual);
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
                    AuthorId = FakeUser.Id,
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
