using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.ControllersTest
{
    public class AccountsTests: IClassFixture<CustomWebApplicationFactory<ScreamBackend.Startup>>
    {
        private readonly CustomWebApplicationFactory<ScreamBackend.Startup> _factory;
        public AccountsTests(CustomWebApplicationFactory<ScreamBackend.Startup> _factory)
        {
            this._factory = _factory;
        }

        [Fact]
        public async void Register_Account_Return_BadRequestResult()
        {
            //  Arrange
            var client = _factory.CreateClient();

            //  Act
            var response = await client.PostAsync("", null);

            //  Assert
            
        }
    }
}
