using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using WebsiteDataCounter.Data.WebsiteCounter;

namespace WebsiteDataCounter
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IWebsiteContentCounterRepository, WebsiteContentCounterRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}