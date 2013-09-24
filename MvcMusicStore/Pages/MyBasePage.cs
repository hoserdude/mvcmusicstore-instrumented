using Microsoft.Practices.Unity;
using MvcMusicStore.Services;

namespace MvcMusicStore.Pages
{
    //TODO inject this for fun
    public class MyBasePage : System.Web.Mvc.ViewPage<ViewModels.StoreBrowseViewModel>
    {
        [Dependency]
        public IMessageService MessageService { get; set; }
    }
}