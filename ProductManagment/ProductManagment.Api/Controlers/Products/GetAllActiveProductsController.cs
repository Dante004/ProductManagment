using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/products")]
    [ApiController]
    public class GetAllActiveProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetAllActiveProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllActive(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllActiveProductsQuery { Paging = new Paging { PageNumber = pageNumber, PageSize = pageSize } }, cancellationToken);
            return result.Process(ModelState);
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class GetAllActiveProductsQuery : IRequest<PaginationResult<ProductDto>>
        {
            public Paging Paging { get; set; }
        }

        public class GetAllActiveProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, PaginationResult<ProductDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public GetAllActiveProductsQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<PaginationResult<ProductDto>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
            {
                return await _dataContext.Products.Where(p => p.IsActive).ToPagedListAsync<Product, ProductDto>(request.Paging, _mapper, cancellationToken);
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
    }
}