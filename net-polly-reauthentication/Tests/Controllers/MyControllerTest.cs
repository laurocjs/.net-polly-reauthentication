using API.Controllers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
    public class MyControllerTest
    {
        private Mock<ISomeService> _someService;

        [Fact]
        public async Task Get_NullList_NotFound()
        {
            var controller = CreateController();

            _someService.Setup(service => service.GetSomething()).Returns(Task.FromResult<List<string>>(null));

            var result = await controller.List();
            var objectResult = (NotFoundObjectResult)result;

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.False(string.IsNullOrEmpty((string)objectResult.Value));
        }

        [Fact]
        public async Task Get_EmptyList_NotFound()
        {
            var controller = CreateController();

            _someService.Setup(service => service.GetSomething()).ReturnsAsync(new List<string>());

            var result = await controller.List();
            var objectResult = (NotFoundObjectResult)result;

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.False(string.IsNullOrEmpty((string)objectResult.Value));
        }

        [Fact]
        public async Task Get_NotEmptyList_Ok()
        {
            var controller = CreateController();

            _someService.Setup(service => service.GetSomething())
                .ReturnsAsync(new List<string>()
                {
                    "one", "two", "three"
                });

            var result = await controller.List();
            var objectResult = (OkObjectResult)result;
            var valueObjectResult = (List<string>)objectResult.Value;

            Assert.NotNull(valueObjectResult);
            Assert.NotEmpty(valueObjectResult);
        }

        private MyController CreateController()
        {
            _someService = new Mock<ISomeService>();
            return new MyController(_someService.Object);
        }
    }
}
