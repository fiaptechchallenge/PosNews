using AutoMapper;
using FluentAssertions;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PosNews.Controllers;
using PosNews.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PosNews_Testes.Unit.Application.Noticias
{
    public class GetAllAsyncTests
    {
        [Fact]
        public async Task ListaNoticiaRetornaOK()
        {
            var noticiaRepository = Mock.Of<INoticiaRepository>();
            var mapper = Mock.Of<IMapper>();

            var noticias = new List<Noticia>()
            {
                new Noticia("Titulo Teste 1", "Descrição Teste", "Chapeu Teste", "Autor Teste"),
                new Noticia("Titulo Teste 2", "Descrição Teste", "Chapeu Teste", "Autor Teste"),
                new Noticia("Titulo Teste 3", "Descrição Teste", "Chapeu Teste", "Autor Teste"),
            };

            Mock.Get(noticiaRepository)
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Noticia, bool>>>()))
                .ReturnsAsync(noticias);

            var noticiaController = new NoticiaController(noticiaRepository, mapper);

            var result = await noticiaController.GetAllAsync();

            var objectResult = Assert.IsType<OkObjectResult>(result.Result);

            objectResult.StatusCode.Should().Be(200);
            objectResult.Value.Should().NotBeNull();
        }
    }
}
