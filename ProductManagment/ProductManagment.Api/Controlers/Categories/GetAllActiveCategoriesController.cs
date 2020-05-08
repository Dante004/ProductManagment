using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/categories")]
    [ApiController]
    public class GetAllActiveCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetAllActiveCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllActive(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllActiveCategoriesQuery { Paging = new Paging { PageNumber = pageNumber, PageSize = pageSize } }, cancellationToken);
            return result.Process(ModelState);
        }

        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GetAllActiveCategoriesQuery : IRequest<PaginationResult<CategoryDto>>
        {
            public Paging Paging { get; set; }
        }

        public class GetAllActiveCategoriesQueryHandler : IRequestHandler<GetAllActiveCategoriesQuery, PaginationResult<CategoryDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public GetAllActiveCategoriesQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<PaginationResult<CategoryDto>> Handle(GetAllActiveCategoriesQuery request, CancellationToken cancellationToken)
            {
                return await _dataContext.Categories.Where(c => c.IsActive).ToPagedListAsync<Category, CategoryDto>(request.Paging, _mapper, cancellationToken);
            }
        }

        public class CategoryProfile : Profile
        {
            public CategoryProfile()
            {
                CreateMap<Category, CategoryDto>()
                    .ReverseMap();
            }
        }

        
    }
}