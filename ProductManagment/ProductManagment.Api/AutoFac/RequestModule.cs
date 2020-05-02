using Autofac;
using MediatR;

namespace ProductManagment.Api.AutoFac
{
    public class RequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(IRequest<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(IRequestHandler<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces();
        }
    }
}
