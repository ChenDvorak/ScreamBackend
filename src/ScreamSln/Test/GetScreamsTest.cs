using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    /// <summary>
    /// Test for get screams by class library Scream
    /// </summary>
    public class GetScreamsTest: DBSeedFactory
    {

        /// <summary>
        /// Test for first page
        /// </summary>
        [Fact]
        public async void GetScreams_FirstPageIndexAndSize_ReturnFullDataOfPage()
        {
            //  arrange
            const int INDEX = 1;
            const int SIZE = 10;
            const int TOTAL_PAGE = 2;

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var screams = await screamsManager.GetScreamsAsync(INDEX, SIZE);

            //  assert
            Assert.Equal(screams.Index, INDEX);
            Assert.Equal(screams.Size, SIZE);
            Assert.Equal(screams.TotalSize, FakerScreamModels.Count);
            Assert.Equal(screams.TotalPage, TOTAL_PAGE);
            Assert.Equal(screams.List.Count, SIZE);
        }

        /// <summary>
        /// Test for second page
        /// </summary>
        [Fact]
        public async void GetScreams_SecondPageIndexAndSize_ReturnPartOfDataOfPage()
        {
            //  arrange
            const int INDEX = 2;
            const int SIZE = 10;
            const int TOTAL_PAGE = 2;
            const int RETURN_SIZE = 2;

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var screams = await screamsManager.GetScreamsAsync(INDEX, SIZE);

            //  assert
            Assert.Equal(screams.Index, INDEX);
            Assert.Equal(screams.Size, SIZE);
            Assert.Equal(screams.TotalSize, FakerScreamModels.Count);
            Assert.Equal(screams.TotalPage, TOTAL_PAGE);
            Assert.Equal(screams.List.Count, RETURN_SIZE);
        }

        /// <summary>
        /// Test third page
        /// </summary>
        [Fact]
        public async void GetScreams_ThirdPageIndexAndSize_ReturnEmptyList()
        {
            //  arrange
            const int INDEX = 3;
            const int SIZE = 10;
            const int TOTAL_PAGE = 2;
            const int RETURN_SIZE = 0;

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager
            (
                _db, redisConn
            );

            //  act
            var screams = await screamsManager.GetScreamsAsync(INDEX, SIZE);

            //  assert
            Assert.Equal(screams.Index, INDEX);
            Assert.Equal(screams.Size, SIZE);
            Assert.Equal(screams.TotalSize, FakerScreamModels.Count);
            Assert.Equal(screams.TotalPage, TOTAL_PAGE);
            Assert.Equal(screams.List.Count, RETURN_SIZE);
        }
    }
}
