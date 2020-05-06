using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.GetAllActiveCategoriesController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class GetAllActiveTest
    {
        protected Mock<IMediator> Mediator;
        protected Result<IEnumerable<CategoryDto>> OkResult;
        protected Result<IEnumerable<CategoryDto>> ErrorResult;
        protected IEnumerable<CategoryDto> ProductDto;

        protected GetAllActiveCategoriesController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetAllActiveCategoriesController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            ProductDto = Builder<CategoryDto>.CreateListOfSize(10).Build();

            OkResult = Result.Ok(ProductDto);
            ErrorResult = Result.Error<IEnumerable<CategoryDto>>("Error");

            Mediator.Setup(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Return_Collection_Of_CategoryDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetAllActive();
            //Assert
            result.Should().BeOk((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.GetAllActive();
            //Assert
            result.Should().BeBadRequest((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
