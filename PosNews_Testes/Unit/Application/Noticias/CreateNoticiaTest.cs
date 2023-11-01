using System.ComponentModel;
using AutoMapper;
using FluentAssertions;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PosNews.Controllers;
using PosNews.Interfaces;
using System.Linq.Expressions;
using PosNews.Models.Dto;

namespace PosNews_Testes.Unit.Application.Noticias;

public class CreateNoticiaTest
{
    [Fact]
    [Description("Este teste deve retornar um CreatedAtActionResult")]
    public async Task CreateNoticiaAsyncTestShoudReturnCreate()
    {
        var noticiaMock = Mock.Of<Noticia>();
        var noticiaDto = Mock.Of<NoticiaDto>();
        
        var noticiaRepository = Mock.Of<INoticiaRepository>();
        Mock.Get(noticiaRepository).Setup(repo => repo.CreateAsync(It.IsAny<Noticia>()))
            .Returns(Task.CompletedTask);
        
        var mapper = Mock.Of<IMapper>();
        Mock.Get(mapper).Setup(mapper => mapper.Map<Noticia>(It.IsAny<NoticiaDto>()))
            .Returns(noticiaMock);
        
        var noticiaController = new NoticiaController(noticiaRepository, mapper);
        var result = await noticiaController.CreateNoticiaAsync(noticiaDto);
        var objectResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().NotBeNull();
    }
}