using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Products;
using ProductManagment.Api.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Products.GetAllActiveProductsController;

namespace ProductManagment.Api.Tests.Controllers.ProductsTests
{
    public class GetAllActiveTest
    {
        private Mock<IMediator> Mediator;
        private Result<IEnumerable<ProductDto>> OkResult;
        private Result<IEnumerable<ProductDto>> ErrorResult;
        private IEnumerable<ProductDto> ProductDto;

        protected GetAllActiveProductsController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetAllActiveProductsController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            ProductDto = Builder<ProductDto>.CreateListOfSize(10).Build();

            OkResult = Result.Ok(ProductDto);
            ErrorResult = Result.Error<IEnumerable<ProductDto>>("Error");

            Mediator.Setup(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Return_Collection_Of_ProductDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetAllActive();
            //Assert
            result.Should().BeOk((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.GetAllActive();
            //Assert
            result.Should().BeBadRequest((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
