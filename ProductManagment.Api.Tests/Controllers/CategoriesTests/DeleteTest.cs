using AutoMapper;
using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.DeleteCategoryController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class Delete
    {
        protected Mock<IMediator> Mediator;
        protected Result<int> OkResult;
        protected Result<int> ErrorResult;
        protected int Id;
        protected DeleteCategoryCommand Command;

        protected DeleteCategoryController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new DeleteCategoryController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            Id = Builder<int>.CreateNew().Build();

            OkResult = Result.Ok(Id);
            ErrorResult = Result.Error<int>("Error");

            Command = Builder<DeleteCategoryCommand>.CreateNew().Build();

            Mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Product_Was_Added()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Delete(Command);
            //Assert
            result.Should().BeOk((object)Id);

            Mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Delete(Command);
            //Assert
            result.Should().BeBadRequest((object)Id);

            Mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
