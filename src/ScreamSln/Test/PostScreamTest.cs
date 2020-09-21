using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{

    public class PostScreamTest: DBSeedFactory
    {
        [Fact]
        public async void Post_Scream_Return_Successful()
        {
            //  arrange
            var redisConn = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync("localhost");

            Screams.IScreamsManager screamsManager = new Screams.DefaultScreamsManager(
                _db, redisConn);
            Screams.Models.NewScreamtion fakeNewScream = new Screams.Models.NewScreamtion
            { 
                Author = new ScreamBackend.DB.Tables.User
                { 
                    UserName = "Dvorak",
                    NormalizedUserName = "DVORAK",
                    Email = "dvorak@outlook.com",
                    NormalizedEmail = "DVORAK@OUTLOOK.COM",
                    IsAdmin = false,
                    CreateDateTime = DateTime.Now,
                    Avatar = ""
                },
                Content = "TEST: NEW SCREAM CONTENT"
            };

            //  act
            
            var result = await screamsManager.PostScreamAsync(fakeNewScream);

            //  assert
            Assert.True(result.Successed);
        }
    }
}
