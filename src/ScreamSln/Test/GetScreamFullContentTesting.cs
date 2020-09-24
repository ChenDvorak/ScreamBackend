using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    public class GetScreamFullContentTesting : DBSeedFactory
    {
        private const int SCREAM_ID = 5;

        [Fact]
        public async void GetFullContent_ExistScreamId_ReturnFullContent()
        {
            //  arrange
            const string EXPECT_CONTENT = "TEST: SCREAM ITEM_5";

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            string actualContent = scream.FullContent;

            //  assert
            Assert.Equal(EXPECT_CONTENT, actualContent);
        }
    }
}
