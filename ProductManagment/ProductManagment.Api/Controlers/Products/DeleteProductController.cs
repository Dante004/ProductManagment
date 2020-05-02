using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/[controller]")]
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
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        public class DeleteProductCommand : IRequest<int>
        {
            public int Id { get; set; }
        }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
        {
            private readonly DataContext _dataContext;

            public DeleteProductCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Id = request.Id
                };

                _dataContext.Entry(product).State = EntityState.Deleted;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return request.Id;
            }
        }
    }
}