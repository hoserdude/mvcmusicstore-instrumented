using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Buche;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using MvcMusicStore.Services;
using MvcMusicStore.Factories;
using MvcMusicStore.Controllers;
using MvcMusicStore.Filters;
using log4net.Config;

namespace MvcMusicStore
{
    public class MvcApplication : HttpApplication
    {
        public static ILogger Logger = null;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var container = new UnityContainer();
            //Custom Container Locator, because one is not enough...
            ContainerLocator.Container = container;

            //Aspects enabled
            container.AddNewExtension<Interception>();

            //Controllers
            var factory = new UnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);
            
            //Log config
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "MvcMusicStore.Config.music_store-log4net.xml";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    XmlConfigurator.Configure(stream);
                }
            }

            container.RegisterType<ILogger, Log4NetLoggerAdapter>(new PerResolveLifetimeManager(),
                                                                   new InjectionConstructor(MethodBase.GetCurrentMethod()));
            //Wake up the logger
            Logger = container.Resolve<ILogger>();
            Logger.Info("Stuff Initialized!");

            IDependencyResolver resolver = DependencyResolver.Current;
            IDependencyResolver newResolver = new UnityDependencyResolver(container, resolver);
            DependencyResolver.SetResolver(newResolver);

            //Register Types - add the instrumentation interceptor
            container.RegisterType<IStoreService, StoreService>(new ContainerControlledLifetimeManager(),
                                                                         new Interceptor<InterfaceInterceptor>(),
                                                                         new InterceptionBehavior<InstrumentationInterceptor>());
            container.RegisterType<IController, StoreController>("Store");

            container.RegisterType<IViewPageActivator, CustomViewPageActivator>(new InjectionConstructor(container));

            container.RegisterInstance<IFilterProvider>("FilterProvider", new FilterProvider(container));
            container.RegisterInstance<IActionFilter>("TraceActionFilter", new TraceActionFilter());
        }

        protected void Session_Start()
        {
            // set the log requestId here also, because this is the start of the session
            Logger.SetRequestId(Session.SessionID, Request.GetHashCode().ToString());
        }
    }
}