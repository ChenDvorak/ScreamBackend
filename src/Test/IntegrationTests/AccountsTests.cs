using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class AccountsTests: IClassFixture<WebApplicationFactory<ScreamBackend.Startup>>
    {
        private readonly WebApplicationFactory<ScreamBackend.Startup> _factory;
        public AccountsTests(WebApplicationFactory<ScreamBackend.Startup> _factory)
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
