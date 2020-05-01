using ProductManagment.Api.Interfaces.Repositories;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
