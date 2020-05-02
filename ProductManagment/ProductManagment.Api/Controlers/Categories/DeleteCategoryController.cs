using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/[controller]")]
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
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        public class DeleteCategoryCommand : IRequest<int>
        {
            public int Id { get; set; }
        }

        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
        {
            private readonly DataContext _dataContext;

            public DeleteCategoryCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category
                {
                    Id = request.Id
                };

                _dataContext.Entry(category).State = EntityState.Deleted;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return request.Id;
            }
        }
    }
}