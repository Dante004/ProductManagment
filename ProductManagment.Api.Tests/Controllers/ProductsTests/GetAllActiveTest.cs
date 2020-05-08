using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Products;
using ProductManagment.Api.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Products.GetAllActiveProductsController;

namespace ProductManagment.Api.Tests.Controllers.ProductsTests
{
    public class GetAllActiveTest
    {
        private Mock<IMediator> mediator;
        private PaginationResult<ProductDto> okResult;
        private IEnumerable<ProductDto> productDto;
        private int pageSize;
        private int pageNumber;

        protected GetAllActiveProductsController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetAllActiveProductsController(mediator.Object);
        }

        private void CorrectFlow()
        {
            productDto = Builder<ProductDto>.CreateListOfSize(10).Build();

            okResult = Builder<PaginationResult<ProductDto>>.CreateNew()
                .With(p => p.Success = true)
                .With(p => p.Items = productDto)
                .With(p => p.Count = productDto.Count())
                .With(p => p.Page = pageNumber)
                .With(p => p.Size = pageSize)
                .Build();

            mediator.Setup(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Return_Collection_Of_ProductDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetAllActive(pageNumber, pageSize);
            //Assert
            result.Should().BeOk(okResult);

            mediator.Verify(m => m.Send(It.IsAny<GetAllActiveProductsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
