using AutoMapper;
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
        protected Mock<IMediator> Mediator;
        protected Result<CategoryDto> OkResult;
        protected Result<CategoryDto> ErrorResult;
        protected CategoryDto ProductDto;

        protected GetCategoryController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetCategoryController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            ProductDto = Builder<CategoryDto>.CreateNew().Build();

            OkResult = Result.Ok(ProductDto);
            ErrorResult = Result.Error<CategoryDto>("Error");

            Mediator.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Return_CategoryDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeOk((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Get(10);
            //Assert
            result.Should().BeBadRequest((object)ProductDto);

            Mediator.Verify(m => m.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
