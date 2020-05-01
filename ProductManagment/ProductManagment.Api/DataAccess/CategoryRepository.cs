using ProductManagment.Api.Interfaces.Repositories;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
