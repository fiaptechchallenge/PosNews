using AutoMapper;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Identity;
using PosNews.Models;
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
