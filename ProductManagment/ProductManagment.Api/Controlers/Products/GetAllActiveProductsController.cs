using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Products
{
    [Route("api/product")]
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
            var result = await _mediator.Send(new GetAllActiveProductsQuery(), cancellationToken);
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

        public class GetAllActiveProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
        {

        }

        public class GetAllActiveProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, Result<IEnumerable<ProductDto>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public GetAllActiveProductsQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
            {
                var products =  _dataContext.Products.Where(p => p.IsActive);

                var productDtos = await _mapper.ProjectTo<ProductDto>(products).ToListAsync(cancellationToken);

                return Result.Ok(productDtos.AsEnumerable());
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