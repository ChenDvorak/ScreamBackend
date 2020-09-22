using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    public class RemoveScreamTest : DBSeedFactory
    {

        [Theory]
        [InlineData(12)]
        [InlineData(11)]
        [InlineData(10)]
        [InlineData(9)]
        public async void Remove_ReturnSuccessful(int screamId)
        {
            //  arrange

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var result = await screamsManager.RemoveAsync(screamId);

            //  assert
            Assert.True(result.Successed);
        }
    }
}
