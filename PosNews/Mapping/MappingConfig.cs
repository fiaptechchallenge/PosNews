using AutoMapper;
using Infraestrutura.Models;
using PosNews.Models.Dto;

namespace PosNews.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Noticia, NoticiaDto>().ReverseMap();
        }
    }
}
