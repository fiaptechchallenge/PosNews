using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PosNews.Controllers;
using PosNews.Models;
using PosNews.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNews_Testes.Unit.Application.Users
{
    public class RegisterUserTests
    {
        [Fact]
        public async Task RegistraUsuario()
        {
            var authService = Mock.Of<IAuthService>();

            var usuario = new RegisterUser
            {
                UserName = "Test",
                Password = "Password@123",
                Role = "user"
            };

            var userController = new UserController(authService);

            Mock.Get(authService)
                .Setup(u => u.RegisterUser(usuario))
                .ReturnsAsync(true);

            var result = await userController.RegisterUser(usuario);

            var objectResult = Assert.IsType<OkResult>(result);

            Mock.Get(authService)
                .Verify(u => u.RegisterUser(It.IsAny<RegisterUser>()), Times.Once());

            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task SeUsuarioTiverErroAoRegistrarRetornaBadRequest()
        {
            var authService = Mock.Of<IAuthService>();

            var usuario = new RegisterUser
            {
                UserName = "Test",
                Password = "123",
                Role = "user"
            };

            var userController = new UserController(authService);

            Mock.Get(authService)
                .Setup(u => u.RegisterUser(usuario))
                .ReturnsAsync(false);

            var result = await userController.RegisterUser(usuario);

            var objectResult = Assert.IsType<BadRequestResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }
    }
}
