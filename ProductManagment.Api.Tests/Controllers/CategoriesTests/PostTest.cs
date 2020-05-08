using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.AddOrUpdateCategoryController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class PostTest
    {
        private Mock<IMediator> mediator;
        private Result<int> okResult;
        private Result<int> errorResult;
        private int id;
        private AddOrUpdateCategoryCommand command;

        protected AddOrUpdateCategoryController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new AddOrUpdateCategoryController(mediator.Object);
        }

        private void CorrectFlow()
        {
            id = Builder<int>.CreateNew().Build();

            okResult = Result.Ok(id);
            errorResult = Result.Error<int>("Error");

            command = Builder<AddOrUpdateCategoryCommand>.CreateNew().Build();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Category_Was_Added()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Post(command);
            //Assert
            result.Should().BeSuccess((object)id);

            mediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResult);
            //Act
            var result = await controller.Post(command);
            //Assert
            result.Should().BeBadRequest((object)id);

            mediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
