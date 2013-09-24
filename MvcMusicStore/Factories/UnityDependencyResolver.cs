using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MvcMusicStore.Factories
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        readonly IUnityContainer _container;
        readonly IDependencyResolver _resolver;

        public UnityDependencyResolver(IUnityContainer container, IDependencyResolver resolver)
        {
            this._container = container;
            this._resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return _resolver.GetService(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch
            {
                return _resolver.GetServices(serviceType);
            }
        }
    }
}