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
        public async Task<IActionResult> Post([FromBody] InsertProductCommand command, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Post), product);
        }

        public class InsertProductCommand : IRequest<int>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, int>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public InsertProductCommandHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<int> Handle(InsertProductCommand request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request);

                await _dataContext.Products.AddAsync(product, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return product.Id;
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

                RuleFor(p => p.Category)
                    .NotNull();
            }
        }
    }
}