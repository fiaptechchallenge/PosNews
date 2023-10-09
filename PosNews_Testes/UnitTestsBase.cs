using AutoMapper;

namespace PosNews_Testes
{
    public class UnitTestsBase : IDisposable
    {
        public IMapper mapper;
        private MapperConfiguration _config;

        public UnitTestsBase()
        {
            _config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            mapper = _config.CreateMapper();
        }

        public void Dispose()
        {
        }
    }
}
