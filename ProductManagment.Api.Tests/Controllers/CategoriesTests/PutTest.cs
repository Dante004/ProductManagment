﻿using FizzWare.NBuilder;
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
    public class PutTest
    {
        private Mock<IMediator> mediator;
        private Result<int> OkResult;
        private Result<int> ErrorResult;
        private int Id;
        private AddOrUpdateCategoryCommand Command;

        protected AddOrUpdateCategoryController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new AddOrUpdateCategoryController(mediator.Object);
        }

        private void CorrectFlow()
        {
            Id = Builder<int>.CreateNew().Build();

            OkResult = Result.Ok(Id);
            ErrorResult = Result.Error<int>("Error");

            Command = Builder<AddOrUpdateCategoryCommand>.CreateNew()
                .With(u => u.Id = 1).Build();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Category_Was_Updated()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Put(Command);
            //Assert
            result.Should().BeOk((object)Id);

            mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Put(Command);
            //Assert
            result.Should().BeBadRequest((object)Id);

            mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
