using FluentAssertions;
using FluentAssertions.Execution;
using Infraestrutura.Contexts;
using Infraestrutura.Models;
using Newtonsoft.Json;
using PosNews.Models.Dto;
using PosNews_Testes.Builders;

namespace PosNews_Testes.Integration
{
    public class NoticiaControlleTeste : IClassFixture<IntegrationTestsBase<ApplicationDbContext>>
    {
        private IntegrationTestsBase<ApplicationDbContext> _integrationaBase;
        private ApplicationDbContext _context;
        private HttpClient _client;

        public NoticiaControlleTeste(IntegrationTestsBase<ApplicationDbContext> integrationBase)
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
    }
}
