using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using UklonBot.Helpers;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.Repositories.Exact;

namespace UklonBot.Infrastructure
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<CommunicationHelper>().As<ICommunicationHelper>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}