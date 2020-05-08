using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;
using System;

namespace ProductManagment.Migrator
{
    class Program
    {

        static void Main(string[] args)
        {
            Migrate(Environment.GetEnvironmentVariable("DbConnection"));
        }

        private static void Migrate(string value)
        {
            var config = new DbContextOptionsBuilder<DataContext>();
            config
                .UseSqlServer(value);

            using (var context = new DataContext(config.Options))
            {
                context.Database.Migrate();
            }
        }
    }
}
