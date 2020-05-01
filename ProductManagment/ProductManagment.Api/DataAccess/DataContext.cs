using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
    }
}
