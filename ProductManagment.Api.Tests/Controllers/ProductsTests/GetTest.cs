using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Products;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Products.GetProductController;

namespace ProductManagment.Api.Tests.Controllers.ProductsTests
{
    public class GetTest
    {
        private Mock<IMediator> mediator;
        private Result<ProductDto> okResult;
        private Result<ProductDto> errorResult;
        private ProductDto productDto;

        protected GetProductController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetProductController(mediator.Object);
        }

        private void CorrectFlow()
        {
            productDto = Builder<ProductDto>.CreateNew().Build();

            okResult = Result.Ok(productDto);
            errorResult = Result.Error<ProductDto>("Error");

            mediator.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Return_ProductDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeOk((object)productDto);

            mediator.Verify(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResult);
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeBadRequest((object)productDto);

            mediator.Verify(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
