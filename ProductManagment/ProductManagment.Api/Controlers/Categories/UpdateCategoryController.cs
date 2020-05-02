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

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UpdateCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        public class UpdateCategoryCommand : IRequest<int>
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
        {
            private DataContext _dataContext;

            public UpdateCategoryCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category
                {
                    Id = request.Id
                };

                _dataContext.Categories.Attach(category);

                category.Name = request.Name;

                await _dataContext.SaveChangesAsync(cancellationToken);

                return request.Id;
            }
        }
    }
}