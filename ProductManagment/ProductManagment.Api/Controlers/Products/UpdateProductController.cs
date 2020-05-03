using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
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
            var result = await _mediator.Send(command, cancellationToken);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        public class UpdateProductCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IValidator<Product> _validator;

            public UpdateProductCommandHandler(DataContext dataContext,
                IValidator<Product> validator)
            {
                _dataContext = dataContext;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

                var result = await _validator.ValidateAsync(product, cancellationToken);

                if(!result.IsValid)
                {
                    return Result.Error<int>(result.Errors);
                }

                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(request.Id);
            }
        }

        public class ProductValidtaor : AbstractValidator<Product>
        {
            public ProductValidtaor()
            {
                RuleFor(p => p.Name)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Length(3, 25);

                RuleFor(p => p.Description)
                    .Length(3, 100);

                RuleFor(p => p.Price)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .ScalePrecision(2, 5)
                    .GreaterThanOrEqualTo(0);

                RuleFor(p => p.CategoryId)
                    .NotNull();
            }
        }
    }
}