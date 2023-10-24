using Bogus;
using PosNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNews_Testes.Builders
{
    public class RegisterNoticiaBuilder : Faker<RegisterNoticia>
    {
        public RegisterNoticiaBuilder()
        {
            RuleFor(n => n.Titulo, f => f.Lorem.Text());
            RuleFor(n => n.Descricao, f => f.Lorem.Paragraph());
            RuleFor(n => n.Chapeu, f => f.Name.FullName());
            RuleFor(n => n.DataPublicacao, DateTime.Now);
            RuleFor(n => n.Autor, f => f.Name.FullName());
        }
    }
}
