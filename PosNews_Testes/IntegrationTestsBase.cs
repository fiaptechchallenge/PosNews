using AutoMapper;
using Infraestrutura.Contextes;
using Infraestrutura.Contexts;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosNews;
using PosNews.Models;
using PosNews_Testes.Builders;
using System.Net.Http.Json;
using WireMock.Server;

namespace PosNews_Testes
{
    public class IntegrationTestsBase : IDisposable
    {
        public IMapper mapper;
        private MapperConfiguration _config;
        private ApplicationDbContext _dataContext;
        private AuthDbContext _authContext;
        private SqliteConnection _connection;
        private HttpClient _client;
        private string _token;

        public IntegrationTestsBase()
        {
            _config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            mapper = _config.CreateMapper();
            WireMockServer.Start(58116);

            var webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((_, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.Testing.json", true, true);
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseEnvironment("Testing");
                });

            var scope = webApplicationFactory.Services.CreateScope();

            _authContext = scope.ServiceProvider.GetService<AuthDbContext>();
            _dataContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "admin", "user" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            _client = webApplicationFactory.CreateClient();
            _token = GenerateToken().Result;

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
        }

        public async Task<string> GenerateToken()
        {
            var regisUser = new RegisterUser()
            {
                UserName = "Rihana",
                Password = "FragoEmpanado@123",
                Role = "admin"
            };

            var loginUser = new LoginUser()
            {
                UserName = regisUser.UserName,
                Password = regisUser.Password
            };

            await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));
            var tokenResult = await _client.PostAsync("api/User/Login", JsonContent.Create(loginUser));
            return await tokenResult.Content.ReadAsStringAsync();
        }

        public ApplicationDbContext GetContext() => _dataContext;

        public HttpClient GetHttpClient() => _client;

        public string GetToken() => _token;

        public void Dispose()
        {
            _authContext.Database.EnsureDeleted();
            _authContext.Database.EnsureCreated();
        }
    }
}
