using System.Threading;
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
    [Route("api/products")]
    [ApiController]
    public class AddOrUpdateProductController : ControllerBase
    {
        private IMediator _mediator;

        public AddOrUpdateProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddOrUpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Process(ModelState, nameof(Post));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AddOrUpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.Process(ModelState);
        }

        public class AddOrUpdateProductCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class AddOrUpdateProductCommandHandler : IRequestHandler<AddOrUpdateProductCommand, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IValidator<Product> _validator;

            public AddOrUpdateProductCommandHandler(DataContext dataContext,
                IMapper mapper,
                IValidator<Product> validator)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(AddOrUpdateProductCommand request, CancellationToken cancellationToken)
            {
                if(request.Id == 0)
                {
                    var product = _mapper.Map<Product>(request);

                    var result = _validator.Validate(product);

                    if (!result.IsValid)
                    {
                        return Result.Error<int>(result.Errors);
                    }

                    await _dataContext.Products.AddAsync(product, cancellationToken);
                    await _dataContext.SaveChangesAsync(cancellationToken);

                    return Result.Ok(product.Id);
                }
                else
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

                    if (!result.IsValid)
                    {
                        return Result.Error<int>(result.Errors);
                    }

                    await _dataContext.SaveChangesAsync(cancellationToken);

                    return Result.Ok(request.Id);
                }

            }
        }

        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, AddOrUpdateProductCommand>()
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