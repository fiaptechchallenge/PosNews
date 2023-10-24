using Bogus;
using Infraestrutura.Models;

namespace PosNews_Testes.Builders
{
    public class NoticiaBuilder : Faker<Noticia>
    {
        public NoticiaBuilder()
        {
            RuleFor(n => n.Titulo, f => f.Lorem.Text());
            RuleFor(n => n.Descricao, f => f.Lorem.Paragraph());
            RuleFor(n => n.Chapeu, f => f.Name.FullName());
            RuleFor(n => n.Autor, f => f.Name.FullName());
        }
    }
}
