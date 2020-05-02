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

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllActiveCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetAllActiveCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllActiveCategoriesQuery(), cancellationToken);
            return Ok(result.Value);
        }

        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GetAllActiveCategoriesQuery : IRequest<Result<IEnumerable<CategoryDto>>>
        {

        }

        public class GetAllActiveCategoriesQueryHandler : IRequestHandler<GetAllActiveCategoriesQuery, Result<IEnumerable<CategoryDto>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public GetAllActiveCategoriesQueryHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetAllActiveCategoriesQuery request, CancellationToken cancellationToken)
            {
                var categories =  _dataContext.Categories.Where(p => p.IsActive);

                var categoryDtos = await _mapper.ProjectTo<CategoryDto>(categories).ToListAsync(cancellationToken);

                return Result.Ok(categoryDtos.AsEnumerable());
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