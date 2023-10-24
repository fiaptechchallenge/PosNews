using AutoMapper;
using FluentAssertions;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PosNews.Controllers;
using PosNews.Interfaces;
using System.Linq.Expressions;


namespace PosNews_Testes.Unit.Application.Noticias
{
    public class GetNoticiaByIdTests
    {
        [Fact]
        public async Task GetNoticia()
        {
            var noticiaRepository = Mock.Of<INoticiaRepository>();
            var mapper = Mock.Of<IMapper>();
            var id = 1;

            var noticia = new Noticia("Titulo Teste", "Descrição Teste", "Chapeu Teste", "Autor Teste");
            noticia.Id = id;

            Mock.Get(noticiaRepository)
                .Setup(n => n.GetAsync(It.IsAny<Expression<Func<Noticia, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(noticia);

            var noticiaController = new NoticiaController(noticiaRepository, mapper);

            var result = await noticiaController.GetNoticiaById(id);

            var objectResult = Assert.IsType<OkObjectResult>(result.Result);

            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task RetornaNotFoundSeNaoExistirNoticia()
        {
            var noticiaRepository = Mock.Of<INoticiaRepository>();
            var mapper = Mock.Of<IMapper>();
            var id = 1;

            Mock.Get(noticiaRepository)
                .Setup(n => n.GetAsync(It.IsAny<Expression<Func<Noticia, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Noticia?)null!);


            var noticiaController = new NoticiaController(noticiaRepository, mapper);

            var result = await noticiaController.GetNoticiaById(id);

            var objectResult = Assert.IsType<NotFoundResult>(result.Result);

            objectResult.StatusCode.Should().Be(404);
            result.Value.Should().BeNull();
        }
    }
}
