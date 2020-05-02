using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UpdateProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        public class UpdateProductCommand : IRequest<int>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
        {
            private DataContext _dataContext;

            public UpdateProductCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Id = request.Id
                };

                _dataContext.Products.Attach(product);

                product.Name = request.Name;
                product.Description = request.Description;
                product.CategoryId = request.CategoryId;
                product.Price = request.Price;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return request.Id;
            }
        }
    }
}