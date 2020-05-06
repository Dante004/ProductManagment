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
        private Mock<IMediator> Mediator;
        private Result<ProductDto> OkResult;
        private Result<ProductDto> ErrorResult;
        private ProductDto ProductDto;

        protected GetProductController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetProductController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            ProductDto = Builder<ProductDto>.CreateNew().Build();

            OkResult = Result.Ok(ProductDto);
            ErrorResult = Result.Error<ProductDto>("Error");

            Mediator.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Return_ProductDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeOk((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeBadRequest((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
