using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.DeleteCategoryController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class Delete
    {
        private Mock<IMediator> mediator;
        private Result<int> okResult;
        private Result<int> errorResult;
        private int Id;
        private DeleteCategoryCommand Command;

        protected DeleteCategoryController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new DeleteCategoryController(mediator.Object);
        }

        private void CorrectFlow()
        {
            Id = Builder<int>.CreateNew().Build();

            okResult = Result.Ok(Id);
            errorResult = Result.Error<int>("Error");

            Command = Builder<DeleteCategoryCommand>.CreateNew()
                .With(x => x.Id = Id).Build();

            mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Category_Was_Deleted()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Delete(Command);
            //Assert
            result.Should().BeOk((object)Id);

            mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResult);
            //Act
            var result = await controller.Delete(Command);
            //Assert
            result.Should().BeBadRequest((object)Id);

            mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
