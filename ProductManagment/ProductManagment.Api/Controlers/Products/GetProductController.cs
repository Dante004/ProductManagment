using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var productDto = await _mediator.Send(new GetProductQuery { Id = id }, cancellationToken);
            return Ok(productDto);
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class GetProductQuery : IRequest<ProductDto>
        {
            public int Id { get; set; }
        }

        public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper; 

            public GetProductQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if(product == null)
                {
                    return null;
                }

                var productDto = _mapper.Map<ProductDto>(product);
                return productDto;
            }

            public class ProductProfile : Profile
            {
                public ProductProfile()
                {
                    CreateMap<Product, ProductDto>()
                        .ReverseMap();
                }
            }
        }
    }
}