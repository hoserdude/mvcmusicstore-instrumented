using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MvcMusicStore.Factories
{
    public class CustomViewPageActivator : IViewPageActivator
    {
        readonly IUnityContainer _container;

        public CustomViewPageActivator(IUnityContainer container)
        {
            _container = container;
        }

        public object Create(ControllerContext controllerContext, Type type)
        {
            return _container.Resolve(type);
        }
    }
}