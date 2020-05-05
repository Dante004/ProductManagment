using CommandLine;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.DataAccess;

namespace ProductManagment.Migrator
{
    class Program
    {

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);

            result
                .WithParsed(r => Migrate(r));
        }

        private static void Migrate(Options options)
        {
            var config = new DbContextOptionsBuilder<DataContext>();
            config
                .UseSqlServer(options.ConnectionString);

            using (var context = new DataContext(config.Options))
            {
                context.Database.Migrate();
            }
        }
    }
}
