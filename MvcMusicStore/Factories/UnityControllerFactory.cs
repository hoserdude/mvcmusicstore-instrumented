using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;

namespace MvcMusicStore.Factories
{
    public class UnityControllerFactory : IControllerFactory
    {
        private readonly IUnityContainer _container;
        private readonly IControllerFactory _innerFactory;

        public UnityControllerFactory(IUnityContainer container)
            : this(container, new DefaultControllerFactory())
        {
        }

        protected UnityControllerFactory(IUnityContainer container, IControllerFactory innerFactory)
        {
            _container = container;
            _innerFactory = innerFactory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                return _container.Resolve<IController>(controllerName);
            }
            catch (Exception)
            {
                return _innerFactory.CreateController(requestContext, controllerName);
            }
        }

        public void ReleaseController(IController controller)
        {
            _container.Teardown(controller);
        }


        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return System.Web.SessionState.SessionStateBehavior.Default;
        }
    }
}