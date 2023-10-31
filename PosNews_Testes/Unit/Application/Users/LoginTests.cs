using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PosNews.Controllers;
using PosNews.Models;
using PosNews.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PosNews_Testes.Unit.Application.Users
{
    public class LoginTests
    {
        [Fact]
        public async Task FazLoginUsuarioRetonaOK()
        {
            var authService = Mock.Of<IAuthService>();

            var usuario = new LoginUser
            {
                UserName = "Test",
                Password = "Password@123",
            };

            var userController = new UserController(authService);

            Mock.Get(authService)
                .Setup(u => u.Login(usuario))
                .ReturnsAsync(true);

            var result = await userController.Login(usuario);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Mock.Get(authService)
                .Verify(u => u.Login(It.IsAny<LoginUser>()), Times.Once());

            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task SeUsuarioTiverErroAoLogarRetornaBadRequest()
        {
            var authService = Mock.Of<IAuthService>();

            var usuario = new LoginUser
            {
                UserName = "Test",
                Password = "Password@123",
            };

            var userController = new UserController(authService);

            Mock.Get(authService)
                .Setup(u => u.Login(usuario))
                .ReturnsAsync(false);

            var result = await userController.Login(usuario);

            var objectResult = Assert.IsType<BadRequestResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task SeModelStateTiverErroAoLogarRetornaBadRequest()
        {
            var authService = Mock.Of<IAuthService>();

            var usuario = new LoginUser
            {
                UserName = "Test",
                Password = "Password@123",
            };

            var userController = new UserController(authService);

            userController.ModelState.AddModelError("fakeError", "fakeError");

            var result = await userController.Login(usuario);

            var objectResult = Assert.IsType<BadRequestResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }
    }
}
