using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Products;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Products.AddOrUpdateProductController;

namespace ProductManagment.Api.Tests.Controllers.ProductsTests
{
    public class PutTest
    {
        private Mock<IMediator> mediator;
        private Result<int> okResult;
        private Result<int> errorResult;
        private int id;
        private AddOrUpdateProductCommand command;

        protected AddOrUpdateProductController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new AddOrUpdateProductController(mediator.Object);
        }

        private void CorrectFlow()
        {
            id = Builder<int>.CreateNew().Build();

            okResult = Result.Ok(id);
            errorResult = Result.Error<int>("Error");

            command = Builder<AddOrUpdateProductCommand>.CreateNew()
                .With(u => u.Id = 1).Build();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Product_Was_Updated()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Put(command);
            //Assert
            result.Should().BeOk((object)id);

            mediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResult);
            //Act
            var result = await controller.Put(command);
            //Assert
            result.Should().BeBadRequest((object)id);

            mediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
