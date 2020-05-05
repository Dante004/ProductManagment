using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.Models;

namespace ProductManagment.Api.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}