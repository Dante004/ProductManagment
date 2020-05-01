using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddProductController : ControllerBase
    {
        private IMediator _mediator;

        public AddProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(new InsertUserCommand { ProductDto = productDto });
            return CreatedAtAction(nameof(Post), product);
        }

        public class ProductDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class InsertUserCommand : IRequest<ProductDto>
        {
            public ProductDto ProductDto { get; set; }
        }

        public class InsertUserCommandHandler : IRequestHandler<InsertUserCommand, ProductDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public InsertUserCommandHandler(DataContext dataContext,
                IMediator mediator,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(InsertUserCommand request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request.ProductDto);

                await _dataContext.Products.AddAsync(product, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                var productDto = _mapper.Map<ProductDto>(product);
                return productDto;
            }
        }

        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, ProductDto>()
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

                RuleFor(p => p.Category)
                    .NotNull();
            }
        }
    }
}