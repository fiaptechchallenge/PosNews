using Infraestrutura.Models;

namespace PosNews.Interfaces
{
    public interface INoticiaRepository : IRepository<Noticia>
    {
        Task<List<Noticia>> GetAllNoticias();
    }
}
