using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;

namespace ScreamsTest
{
    /// <summary>
    /// Test post function of class library of sceram
    /// </summary>
    public class PostScreamTest: DBSeedFactory
    {
        private const int NOT_DATA = 0;
        /// <summary>
        /// Test post scream shall be successful
        /// </summary>
        [Fact]
        public async void PostScream_RightData_ReturnSuccessful()
        {
            //  arrange
            const string CONTENT = "TEST: NEW SCREAM CONTENT SUCCESSFUL";
            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager(
                _db, redisConn);
            Screams.Models.NewScreamtion fakerNewScream = new Screams.Models.NewScreamtion
            { 
                AuthorId = FakeUser.Id,
                Content = CONTENT
            };

            //  act
            var result = await screamsManager.PostScreamAsync(fakerNewScream);
            var id = result.Data;
            var newScream = _db.Screams.AsNoTracking().SingleOrDefault(s => s.Id == id);

            //  assert
            Assert.True(result.Succeeded);
            Assert.True(id > 0);
            Assert.NotNull(newScream);
            Assert.Equal(newScream.Content, CONTENT);
        }

        /// <summary>
        /// Test shall be unsuccessful if content empty
        /// </summary>
        [Fact]
        public async void PostScream_EmptyContentData_ReturnUnsuccessful()
        {
            //  arrange
            const int ERRORS_COUNT = 1;
            const string ERRORS_CONTENT = "内容不能为空";

            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager(
                _db, redisConn);
            Screams.Models.NewScreamtion fakerNewScream = new Screams.Models.NewScreamtion
            {
                AuthorId = FakeUser.Id,
                Content = ""
            };

            //  act
            var result = await screamsManager.PostScreamAsync(fakerNewScream);

            //  assert
            Assert.False(result.Succeeded);
            Assert.Equal(result.Data, NOT_DATA);
            Assert.NotNull(result.Errors);
            Assert.Equal(result.Errors.Count, ERRORS_COUNT);
            Assert.Equal(result.Errors[0], ERRORS_CONTENT);
        }

        /// <summary>
        /// Test shall be unsuccessful if author not exist
        /// </summary>
        [Fact]
        public async void PostScream_NotExistAuthor_ReturnUnsuccessful()
        {
            //  arrange
            const int ERRORS_COUNT = 1;
            const string ERRORS_CONTENT = "该作者不存在";

            var fakerAuthor = new ScreamBackend.DB.Tables.User
            {
                Id = 0
            };

            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager(
                _db, redisConn);
            Screams.Models.NewScreamtion fakerNewScream = new Screams.Models.NewScreamtion
            {
                AuthorId = fakerAuthor.Id,
                Content = "TEST: NEW SCREAM CONTENT UNSUCCESSFUL"
            };

            //  act
            var result = await screamsManager.PostScreamAsync(fakerNewScream);

            //  assert
            Assert.False(result.Succeeded);
            Assert.Equal(result.Data, NOT_DATA);
            Assert.NotNull(result.Errors);
            Assert.Equal(result.Errors.Count, ERRORS_COUNT);
            Assert.Equal(result.Errors[0], ERRORS_CONTENT);
        }
    }
}
