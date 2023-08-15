 using PosNews.Interfaces;
using Infraestrutura.Models;
using Microsoft.EntityFrameworkCore;
using Infraestrutura.Contexts;

namespace PosNews.Repository
{
    public class NoticiaRepository : Repository<Noticia>, INoticiaRepository
    {
        private readonly ApplicationDbContext _db;

        public NoticiaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Noticia>> GetAllNoticias() =>
            await _db.Noticia.Take(100).ToListAsync();
    }
}
