using System.Web.Http;
using Unity;
using Unity.WebApi;
using posrepository.DTO;
using posrepository;

namespace service
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IProducts, ProductsRepository>();
            container.RegisterType<ISales, SalesRepository>();
            container.RegisterType<ILostItems, LostItemsRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}