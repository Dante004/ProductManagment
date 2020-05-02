using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetCategoryQuery query, CancellationToken cancellationToken = default)
        {
            var categoryDto = await _mediator.Send(query, cancellationToken);
            return Ok(categoryDto);
        }

        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GetCategoryQuery : IRequest<CategoryDto>
        {
            public int Id { get; set; }
        }

        public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper; 

            public GetCategoryQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if(category == null)
                {
                    return null;
                }

                var productDto = _mapper.Map<CategoryDto>(category);
                return productDto;
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
}