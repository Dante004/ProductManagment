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
    public class PostTest
    {
        private Mock<IMediator> Mediator;
        private Result<int> OkResult;
        private Result<int> ErrorResult;
        private int Id;
        private InsertCategoryCommand Command;

        protected AddOrUpdateCategoryController Create()
        {
            Mediator = new Mock<IMediator>();
            CorrectFlow();
            return new AddOrUpdateCategoryController(Mediator.Object);
        }

        private void CorrectFlow()
        {
            Id = Builder<int>.CreateNew().Build();

            OkResult = Result.Ok(Id);
            ErrorResult = Result.Error<int>("Error");

            Command = Builder<InsertCategoryCommand>.CreateNew().Build();

            Mediator.Setup(m => m.Send(It.IsAny<InsertCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OkResult);
        }

        [Fact]
        public async Task Return_200_When_Category_Was_Added()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.Post(Command);
            //Assert
            result.Should().BeSuccess((object)Id);

            Mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_BadRequest_When_Handler_Return_Error()
        {
            //Arrange
            var controller = Create();

            Mediator.Setup(m => m.Send(It.IsAny<InsertCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorResult);
            //Act
            var result = await controller.Post(Command);
            //Assert
            result.Should().BeBadRequest((object)Id);

            Mediator.Verify(m => m.Send(Command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
