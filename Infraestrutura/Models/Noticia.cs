
using System.ComponentModel.DataAnnotations;

namespace Infraestrutura.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Noticia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string Chapeu { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.Now;
        [Required]
        public string Autor { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="descricao"></param>
        /// <param name="chapeu"></param>
        /// <param name="autor"></param>
        public Noticia(string titulo,
                       string descricao,
                       string chapeu,
                       string autor)
        {
            Titulo = titulo;
            Descricao = descricao;
            Chapeu = chapeu;
            Autor = autor;
        }

        public Noticia()
        {
            
        }
    }
}
