using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using System.IO;
using System.Net.Http;
using System.Text;
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
        public async void Register_ExistAccount_Return_BadRequestResult()
        {
            //  Arrange
            var client = _factory.CreateClient();
            Accounts.Models.RegisterInfo info = new Accounts.Models.RegisterInfo
            { 
                Email = Utilities.FakeUser.Email,
                Password = Utilities.FakeUser.PasswordHash,
                ConfirmPassword = Utilities.FakeUser.PasswordHash
            };
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var actualEmail = "";

            //  Act
            var response = await client.PostAsync("/api/client/accounts", content);
            actualEmail = await response.Content.ReadAsStringAsync();

            //  Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(info.Email, actualEmail);
            
        }
    }
}
