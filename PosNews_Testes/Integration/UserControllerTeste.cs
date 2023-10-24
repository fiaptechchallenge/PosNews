using Infraestrutura.Contextes;
using PosNews.Models;
using PosNews_Testes.Builders;
using System.Net;
using System.Net.Http.Json;

namespace PosNews_Testes.Integration
{
    public class UserControllerTeste : IClassFixture<IntegrationTestsBase>
    {
        private readonly IntegrationTestsBase _integrationBase;
        private readonly AuthDbContext _context;
        private readonly HttpClient _client;

        public UserControllerTeste(IntegrationTestsBase integrationBase)
        {
            _integrationBase = integrationBase;
            _context = _integrationBase.GetAuthContext();
            _client = _integrationBase.GetHttpClient();
        }

        [Fact]
        public async Task PostRegisterUserShouldReturnOk()
        {
            // Arrange
            var expectedRegisteredUser = new RegisterUserBuilder().Generate();

            // Act
            var result = await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(expectedRegisteredUser));
            var stringResult = await result.Content.ReadAsStringAsync();
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(stringResult);
        }

        [Fact]
        public async Task PostRegisterExistingUserShouldReturnBadRequest()
        {
            // Arrange
            var regisUser = new RegisterUser()
            {
                UserName = "Rihana2023",
                Password = "FragoEmpanado@123",
                Role = "admin"
            };

            await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));

            // Act
            var result = await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));
            var stringResult = await result.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotEmpty(stringResult);
        }

        [Fact]
        public async Task PostLoginUserShouldReturnOk()
        {
            // Arrange
            var regisUser = new RegisterUser()
            {
                UserName = "Rihana2023",
                Password = "FragoEmpanado@123",
                Role = "admin"
            };
            await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));

            var loginUser = new LoginUser()
            {
                UserName = "Rihana2023",
                Password = "FragoEmpanado@123",
            };

            // Act
            var result = await _client.PostAsync("api/User/Login", JsonContent.Create(loginUser));

            // Assert
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(stringResult);
        }

        [Fact]
        public async Task PostLoginUnregisteredUserShouldReturnBadRequest()
        {
            // Arrange
            var randomUnregisteredUser = new RegisterUserBuilder().Generate();
            var loginUser = new LoginUser()
            {
                UserName = randomUnregisteredUser.UserName,
                Password = randomUnregisteredUser.Password
            };

            // Act
            var result = await _client.PostAsync("api/User/Login", JsonContent.Create(loginUser));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
