using AutoMapper;
using PosNews.Mapping;

namespace PosNews_Testes.Unit
{
    [Trait("Categoria", "Mapeamento")]
    public class MapeamentoNoticiasTests
    {
        [Fact]
        public void NoticiasMapperShouldBeValid()
        {
            MapperConfiguration mapperConfiguration =
                new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfig()));

            IMapper mapper = mapperConfiguration.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
