using System.Collections.Generic;
using System.Linq;
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
    public class GetAllActiveProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetAllActiveProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive(CancellationToken cancellationToken = default)
        {
            var products = await _mediator.Send(new GetAllActiveProductsQuery(), cancellationToken);
            return Ok(products);
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        public class GetAllActiveProductsQuery : IRequest<IEnumerable<ProductDto>>
        {

        }

        public class GetAllActiveProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, IEnumerable<ProductDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public GetAllActiveProductsQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProductDto>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
            {
                var products = await _dataContext.Products.Where(p => p.IsActive).ToListAsync(cancellationToken);

                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                return productDtos;
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