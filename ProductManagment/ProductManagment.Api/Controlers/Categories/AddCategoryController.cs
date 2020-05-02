using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCategoryController : ControllerBase
    {
        private IMediator _mediator;

        public AddCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var category = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Post), category);
        }

        public class InsertCategoryCommand : IRequest<int>
        {
            public string Name { get; set; }
        }

        public class InsertCategoryCommandHandler : IRequestHandler<InsertCategoryCommand, int>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public InsertCategoryCommandHandler(DataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<int> Handle(InsertCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = _mapper.Map<Category>(request);

                await _dataContext.Categories.AddAsync(category, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return category.Id;
            }
        }

        public class CategoryProfile : Profile
        {
            public CategoryProfile()
            {
                CreateMap<Category, InsertCategoryCommand>()
                    .ReverseMap();
            }
        }

        public class CategoryValidtaor : AbstractValidator<Category>
        {
            public CategoryValidtaor()
            {
                RuleFor(p => p.Name)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Length(3, 25);
            }
        }
    }
}