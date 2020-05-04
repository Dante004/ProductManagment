using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/product")]
    [ApiController]
    public class DeleteProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeleteProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Process(ModelState);
        }

        public class DeleteProductCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
        }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<int>>
        {
            private readonly DataContext _dataContext;

            public DeleteProductCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<int>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Id = request.Id
                };

                _dataContext.Entry(product).State = EntityState.Deleted;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(request.Id);
            }
        }
    }
}