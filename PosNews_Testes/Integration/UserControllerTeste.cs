using AutoMapper;
using Infraestrutura.Contextes;
using Microsoft.AspNetCore.Identity;
using PosNews.Models;
using PosNews_Testes.Builders;
using System.Net;
using System.Net.Http.Json;

namespace PosNews_Testes.Integration
{
    [Collection("Testes de User")]
    public class UserControllerTeste : IClassFixture<IntegrationTestsBase<AuthDbContext>>
    {
        private readonly IntegrationTestsBase<AuthDbContext> _integrationBase;
        private readonly AuthDbContext _context;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public UserControllerTeste(IntegrationTestsBase<AuthDbContext> integrationBase)
        {
            _integrationBase = integrationBase;
            _context = _integrationBase.GetAuthContext();
            _client = _integrationBase.GetHttpClient();
            _mapper = _integrationBase.mapper;
        }

        [Fact]
        public async Task PostRegisterUserShouldReturnOk()
        {
            // Arrange
            var expectedRegisteredUser = new RegisterUserBuilder().Generate();
            var ExpectedRegisteredIdentityUser = _mapper.Map<IdentityUser>(expectedRegisteredUser);

            await _context.Set<IdentityUser>().AddAsync(ExpectedRegisteredIdentityUser);
            await _context.SaveChangesAsync();

            var regisUser = new RegisterUser()
            {
                UserName = expectedRegisteredUser.UserName,
                Password = expectedRegisteredUser.Password,
                Role = expectedRegisteredUser.Role
            };

            // Act
            var result = await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));
            var stringResult = await result.Content.ReadAsStringAsync();
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(stringResult);

            _context.Remove(ExpectedRegisteredIdentityUser);
            await _context.SaveChangesAsync();
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

            var ExpectedRegisteredIdentityUser = _mapper.Map<IdentityUser>(regisUser);

            await _context.Set<IdentityUser>().AddAsync(ExpectedRegisteredIdentityUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));
            var stringResult = await result.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotEmpty(stringResult);

            _context.Remove(ExpectedRegisteredIdentityUser);
            await _context.SaveChangesAsync();
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
