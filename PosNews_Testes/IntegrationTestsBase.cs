using AutoMapper;
using Infraestrutura.Contextes;
using Infraestrutura.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosNews;
using PosNews.Models;
using PosNews_Testes.Builders;
using System.Net;
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
        private HttpClient _client;
        private WireMockServer _wireMockServer;
        private string _token;

        public IntegrationTestsBase()
        {
            _config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            mapper = _config.CreateMapper();

            _wireMockServer = WireMockServer.Start();

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

            _dataContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            _authContext = scope.ServiceProvider.GetService<AuthDbContext>();

            _dataContext?.Database.EnsureDeleted();
            _dataContext?.Database.EnsureCreated();

            _authContext?.Database.EnsureDeleted();
            _authContext?.Database.EnsureCreated();

            _client = webApplicationFactory.CreateClient();
            _token = GenerateToken(scope).Result;

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
        }

        public async Task<string> GenerateToken(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "admin" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var regisUser = new RegisterUserBuilder().Generate();

            var loginUser = new LoginUser()
            {
                UserName = regisUser.UserName,
                Password = regisUser.Password
            };

            var tokenResult = await _client.PostAsync("api/User/Login", JsonContent.Create(loginUser));

            if (tokenResult.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                await _client.PostAsync("api/User/Cadastrar", JsonContent.Create(regisUser));
                tokenResult = await _client.PostAsync("api/User/Login", JsonContent.Create(loginUser));
            }

            return await tokenResult.Content.ReadAsStringAsync();
        }

        public ApplicationDbContext GetContext() => _dataContext;

        public HttpClient GetHttpClient() => _client;

        public void Dispose()
        {
            _wireMockServer.Stop();
            _wireMockServer.Dispose();
            //_dataContext.Database.EnsureDeleted();
            //_authContext.Database.EnsureDeleted();
        }
    }
}
