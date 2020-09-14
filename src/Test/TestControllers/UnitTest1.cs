using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace TestControllers
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRegisterAsync()
        {
            var userStore = new Mock<IUserStore<ScreamBackend.DB.Tables.User>>();
            
            var userManagerMock = new Mock<UserManager<ScreamBackend.DB.Tables.User>>(userStore.Object, null, null, null, null, null, null, null, null);

            ScreamBackend.DB.Tables.User userModel = new ScreamBackend.DB.Tables.User
            {
                IsAdmin = false,
                UserName = "myfor",
                Email = "myfor@gmail.com"
            };
            userManagerMock.Setup(u => u.CreateAsync(userModel))
                .Returns(Task.FromResult(IdentityResult.Success));

            var dbMock =
                new ScreamBackend.DB.ScreamDB(
                    new DbContextOptionsBuilder<ScreamBackend.DB.ScreamDB>().Options
                );


            ScreamBackend.Controllers.Identity.AccountsController accounts = new ScreamBackend.Controllers.Identity.AccountsController
                (
                    dbMock, userManagerMock.Object
                );

            var result = accounts.RegisterAsync("");
            Assert.IsTrue(result.GetType().Equals(typeof(BadRequestObjectResult)));

            result = accounts.RegisterAsync("not empty");
            Assert.IsTrue(result.GetType().Equals(typeof(OkResult)));
        }
    }
}