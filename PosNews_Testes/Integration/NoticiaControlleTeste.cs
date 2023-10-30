using FluentAssertions;
using FluentAssertions.Execution;
using Infraestrutura.Contexts;
using Infraestrutura.Models;
using Newtonsoft.Json;
using PosNews.Models.Dto;
using PosNews_Testes.Builders;
using System.Net.Http.Json;
using System.Net;
using PosNews.Models;

namespace PosNews_Testes.Integration
{
    public class NoticiaControlleTeste : IClassFixture<IntegrationTestsBase>
    {
        private IntegrationTestsBase _integrationaBase;
        private ApplicationDbContext _context;
        private HttpClient _client;
        private const int IdCreated = 1;
        private const int IdNotFound = 9999999;

        public NoticiaControlleTeste(IntegrationTestsBase integrationBase)
        {
            _integrationaBase = integrationBase;
            _context = _integrationaBase.GetContext();
            _client = _integrationaBase.GetHttpClient();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllShouldReturnAllRegister()
        {
            var ExpectedNews = new NoticiaBuilder().Generate(5);
            await _context.Set<Noticia>().AddRangeAsync(ExpectedNews);
            await _context.SaveChangesAsync();

            var result = await _client.GetAsync("api/Noticia");
            var stringResult = await result.Content.ReadAsStringAsync();
            var objResult = JsonConvert.DeserializeObject<IEnumerable<NoticiaDto>>(stringResult);

            _context.RemoveRange(ExpectedNews);
            await _context.SaveChangesAsync();

            using (new AssertionScope())
            {
                objResult.Should().HaveCount(ExpectedNews.Count);
                objResult.Should().Contain(objResult);
            }
        }

        [Fact]
        public async Task GetByIdShouldReturnOk()
        {
            var ExpectedNews = new NoticiaBuilder().Generate();
            await _context.Set<Noticia>().AddAsync(ExpectedNews);
            await _context.SaveChangesAsync();

            var result = await _client.GetAsync($"api/Noticia/{IdCreated}");
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(stringResult);
        }

        [Fact]
        public async Task GetByIdShouldReturnNotFound()
        {
            var result = await _client.GetAsync($"api/Noticia/{IdNotFound}");
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotEmpty(stringResult);
        }

        [Fact]
        public async Task PostRegisterNoticiaShouldReturnOk()
        {
            var expectedRegisteredUser = new RegisterNoticiaBuilder().Generate();

            var result = await _client.PostAsync("api/Noticia", JsonContent.Create(expectedRegisteredUser));
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotEmpty(stringResult);
         }

        [Fact]
        public async Task PostRegisterNoticiaShouldReturnBadRequest()
        {
            // Arrange
            var expectedNoticiaBadRequest = new RegisterNoticia
            {
                Autor = "",
                Chapeu = "",
                Descricao = "",
                Titulo = ""
            };

            // Act
            var result = await _client.PostAsync("api/Noticia", JsonContent.Create(expectedNoticiaBadRequest));
            var stringResult = await result.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotEmpty(stringResult);
        }
    }
}
