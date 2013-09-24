using System.Linq;
using System.Web.Mvc;
using Buche;
using MvcMusicStore.Services;

namespace MvcMusicStore.Controllers
{
    public class StoreController : LoggingBaseController
    {
        private IStoreService service;

        public StoreController(IStoreService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            // Create list of genres
            var genresModel = this.service.GetGenres().ToList();

            return View(genresModel);
        }

        public ActionResult Browse(string genre)
        {
            var genreModel = this.service.GetGenreByName(genre);

            return View(genreModel);
        }

        public ActionResult Details(int id)
        {
            var album = this.service.GetAlbum(id);

            return View(album);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            // Create list of genres
            var genres = this.service.GetGenres();

            return PartialView(genres);
        }
    }
}
