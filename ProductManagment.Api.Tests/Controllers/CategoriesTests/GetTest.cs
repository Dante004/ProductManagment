using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.GetCategoryController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class GetTest
    {
        private Mock<IMediator> mediator;
        private Result<CategoryDto> okResult;
        private Result<CategoryDto> errorResult;
        private CategoryDto productDto;

        protected GetCategoryController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetCategoryController(mediator.Object);
        }

        private void CorrectFlow()
        {
            productDto = Builder<CategoryDto>.CreateNew().Build();

            okResult = Result.Ok(productDto);
            errorResult = Result.Error<CategoryDto>("Error");

            mediator.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Return_CategoryDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeOk((object)productDto);

            mediator.Verify(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResult);
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeBadRequest((object)productDto);

            mediator.Verify(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
