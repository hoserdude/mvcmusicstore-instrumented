using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MvcMusicStore.Filters
{
    public class FilterProvider : IFilterProvider
    {
        readonly IUnityContainer _container;

        public FilterProvider(IUnityContainer container)
        {
            _container = container;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            foreach (IActionFilter actionFilter in _container.ResolveAll<IActionFilter>())
                yield return new Filter(actionFilter, FilterScope.First, null);
        }
    }
}