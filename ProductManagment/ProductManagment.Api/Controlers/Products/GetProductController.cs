using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;
using ProductManagment.Api.Properties;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/product")]
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
            var result = await _mediator.Send(new GetProductQuery { Id = id }, cancellationToken);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(result.Value);
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class GetProductQuery : IRequest<Result<ProductDto>>
        {
            public int Id { get; set; }
        }

        public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<ProductDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper; 

            public GetProductQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if(product == null)
                {
                    return Result.Error<ProductDto>(Resource.ProductIdDosentExist);
                }

                var productDto = _mapper.Map<ProductDto>(product);
                return Result.Ok(productDto);
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