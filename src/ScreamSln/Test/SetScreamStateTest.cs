using Screams.Screams;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ScreamsTest
{
    /// <summary>
    /// testing change scream state
    /// </summary>
    public class SetScreamStateTest : DBSeedFactory
    {

        [Fact]
        public async void Pass_NotInput_ReturnSuccessful()
        {
            const int SCREAM_ID = 1;

            //  arrange
            IScreamsManager screamsManager = new DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            await scream.SetPass();

            //  assert
            Assert.Equal(Scream.Status.Passed, scream.State);
        }

        [Fact]
        public async void Recycle_NotInput_ReturnSuccessful()
        {
            const int SCREAM_ID = 1;

            //  arrange
            IScreamsManager screamsManager = new DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            await scream.SetRecycle();

            //  assert
            Assert.Equal(Scream.Status.Recycle, scream.State);
        }

        [Fact]
        public async void WaitToAudit_NotInput_ReturnSuccessful()
        {
            const int SCREAM_ID = 1;

            //  arrange
            IScreamsManager screamsManager = new DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            await scream.SetWaitAudit();

            //  assert
            Assert.Equal(Scream.Status.WaitAudit, scream.State);
        }
    }
}
