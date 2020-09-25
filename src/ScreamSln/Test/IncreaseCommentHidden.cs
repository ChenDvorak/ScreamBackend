using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using Screams.Screams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test
{
    public class IncreaseScreamHidden : DBSeedFactory
    {
        [Fact]
        public async void Increase_NotInput_RetrunIncreasedHiddenCount()
        {
            //  arrange
            const int SCREAM_ID = 1;
            int expected = ExpectedHiddenCount(SCREAM_ID) + 1;
            int actual;
            IScreamsManager screamsManager = new DefaultScreamsManager(_db, redisConn);
            var scream = await screamsManager.GetScreamAsync(SCREAM_ID);

            //  act
            actual = await scream.IncreaseHidden();
            //  assert
            Assert.Equal(expected, actual);
        }

        private int ExpectedHiddenCount(int sceramId)
        {
            using var context = new ScreamDB(contextOptions);
            var scream = context.Screams.AsNoTracking().SingleOrDefault(s => s.Id == sceramId);
            if (scream == null)
                throw new NullReferenceException();
            return scream.HiddenCount;
        }
    }
}
