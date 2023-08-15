using System.ComponentModel.DataAnnotations;

namespace PosNews.Models.Dto
{
    public class NoticiaDto
    {
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string Chapeu { get; set; }
        public DateTime DataPublicacao { get; set; }
        [Required]
        public string Autor { get; set; }
    }
}
