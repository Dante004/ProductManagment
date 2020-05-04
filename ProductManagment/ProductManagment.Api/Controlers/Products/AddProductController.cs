﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/product")]
    [ApiController]
    public class AddProductController : ControllerBase
    {
        private IMediator _mediator;

        public AddProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertProductCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Post), result.Value);
        }

        public class InsertProductCommand : IRequest<Result<int>>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IValidator<Product> _validator;

            public InsertProductCommandHandler(DataContext dataContext,
                IMapper mapper,
                IValidator<Product> validator)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request);

                var result = _validator.Validate(product);

                if(!result.IsValid)
                {
                    return Result.Error<int>(result.Errors);
                }

                await _dataContext.Products.AddAsync(product, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(product.Id);
            }
        }

        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, InsertProductCommand>()
                    .ReverseMap();
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