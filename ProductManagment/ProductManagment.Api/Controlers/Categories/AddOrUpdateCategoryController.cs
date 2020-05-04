using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagment.Api.DataAccess;
using ProductManagment.Api.Helpers;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.Controlers.Categories
{
    [Route("api/category")]
    [ApiController]
    public class AddOrUpdateCategoryController : ControllerBase
    {
        private IMediator _mediator;

        public AddOrUpdateCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Post), result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(result.Value);
        }

        public class InsertCategoryCommand : IRequest<Result<int>>
        {
            public string Name { get; set; }
        }

        public class InsertCategoryCommandHandler : IRequestHandler<InsertCategoryCommand, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IValidator<Category> _validator;

            public InsertCategoryCommandHandler(DataContext dataContext,
                IMapper mapper,
                IValidator<Category> validator)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(InsertCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = _mapper.Map<Category>(request);

                var result = await _validator.ValidateAsync(category, cancellationToken);

                if(!result.IsValid)
                {
                    return Result.Error<int>(result.Errors);
                }

                await _dataContext.Categories.AddAsync(category, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(category.Id);
            }
        }

        public class UpdateCategoryCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<int>>
        {
            private readonly DataContext _dataContext;
            private readonly IValidator<Category> _validator;

            public UpdateCategoryCommandHandler(DataContext dataContext,
                IValidator<Category> validator)
            {
                _dataContext = dataContext;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category
                {
                    Id = request.Id
                };

                _dataContext.Categories.Attach(category);

                category.Name = request.Name;

                var result = await _validator.ValidateAsync(category, cancellationToken);

                if (!result.IsValid)
                {
                    return Result.Error<int>(result.Errors);
                }

                await _dataContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(request.Id);
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