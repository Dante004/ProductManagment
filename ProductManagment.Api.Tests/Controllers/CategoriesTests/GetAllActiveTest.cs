using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagment.Api.Controlers.Categories;
using ProductManagment.Api.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.GetAllActiveCategoriesController;

namespace ProductManagment.Api.Tests.Controllers.CategoriesTests
{
    public class GetAllActiveTest
    {
        private Mock<IMediator> mediator;
        private PaginationResult<CategoryDto> okResult;
        private IEnumerable<CategoryDto> categoryDto;
        private int pageSize;
        private int pageNumber;

        protected GetAllActiveCategoriesController Create()
        {
            mediator = new Mock<IMediator>();
            CorrectFlow();
            return new GetAllActiveCategoriesController(mediator.Object);
        }

        private void CorrectFlow()
        {
            categoryDto = Builder<CategoryDto>.CreateListOfSize(10).Build();

            okResult = Builder<PaginationResult<CategoryDto>>.CreateNew()
                .With(p => p.Success = true)
                .With(p => p.Items = categoryDto)
                .With(p => p.Count = categoryDto.Count())
                .With(p => p.Size = pageSize)
                .Build();

            mediator.Setup(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(okResult);
        }

        [Fact]
        public async Task Return_200_When_Return_Collection_Of_CategoryDto()
        {
            //Arrange
            var controller = Create();
            //Act
            var result = await controller.GetAllActive(pageNumber, pageSize);
            //Assert
            result.Should().BeOk(okResult);

            mediator.Verify(m => m.Send(It.IsAny<GetAllActiveCategoriesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
