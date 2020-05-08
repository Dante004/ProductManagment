using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/categories")]
    [ApiController]
    public class DeleteCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeleteCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Process(ModelState);
        }

        public class DeleteCategoryCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
        }

        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<int>>
        {
            private readonly DataContext _dataContext;

            public DeleteCategoryCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category
                {
                    Id = request.Id
                };

                _dataContext.Entry(category).State = EntityState.Deleted;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(request.Id);
            }
        }
    }
}