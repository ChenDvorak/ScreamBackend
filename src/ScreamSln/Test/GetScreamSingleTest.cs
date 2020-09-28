using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScreamsTest
{
    public class GetScreamSingleTest : DBSeedFactory
    {

        [Fact]
        public async void GetScream_ExistId_ReturnScream()
        {
            //  arrange
            const int EXIST_ID = 1;

            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var scream = await screamsManager.GetScreamAsync(EXIST_ID);

            //  assert
            Assert.NotNull(scream);
        }

        [Fact]
        public async void GetScream_NotExistId_ReturnNull()
        {
            //  arrange
            const int NOT_EXIST_ID = 10111;

            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var scream = await screamsManager.GetScreamAsync(NOT_EXIST_ID);

            //  assert
            Assert.Null(scream);
        }

        [Fact]
        public async void GetScream_InvalidId_ReturnNull()
        {
            //  arrange
            const int INVALID_ID = -1;

            Screams.Screams.IScreamsManager screamsManager = new Screams.Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var scream = await screamsManager.GetScreamAsync(INVALID_ID);

            //  assert
            Assert.Null(scream);
        }
    }
}
