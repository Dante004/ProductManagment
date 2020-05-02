using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagment.Api.DataAccess;

namespace ProductManagment.Api.AutoFac
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<DataContext>();
                opt.UseSqlServer(config.GetConnectionString("Default"));

                return new DataContext(opt.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
