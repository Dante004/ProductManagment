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

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/category")]
    [ApiController]
    public class GetCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetCategoryQuery { Id = id}, cancellationToken);
            return result.Process(ModelState);
        }

        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GetCategoryQuery : IRequest<Result<CategoryDto>>
        {
            public int Id { get; set; }
        }

        public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<CategoryDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper; 

            public GetCategoryQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if(category == null)
                {
                    return Result.Error<CategoryDto>(Resource.CategoryIdDosentExist);
                }

                var productDto = _mapper.Map<CategoryDto>(category);
                return Result.Ok(productDto);
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