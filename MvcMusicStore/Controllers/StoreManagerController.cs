using System;
using System.Linq;
using System.Web.Mvc;
using Buche;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class StoreManagerController : LoggingBaseController
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        public ActionResult Index()
        {
            var albums = storeDB.Albums
                .Include("Genre").Include("Artist")
                .ToList();

            return View(albums);
        }

        public ActionResult Index2()
        {
            var albums = storeDB.Albums
                .Include("Genre").Include("Artist")
                .ToList();

            return View(albums);
        }

        public ViewResult Details(int id)
        {
            Album album = storeDB.Albums.Single(a => a.AlbumId == id);
            return View(album);
        }

        public ActionResult Create()
        {
            var viewModel = new StoreManagerViewModel()
            {
                Album = new Album(),
                Genres = new SelectList(storeDB.Genres.ToList(), "GenreId", "Name"),
                Artists = new SelectList(storeDB.Artists.ToList(), "ArtistId", "Name")
            };
            
            return View(viewModel);
        } 

        [HttpPost]
        public ActionResult Create(Album album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Save Album
                    storeDB.AddToAlbums(album);
                    storeDB.SaveChanges();

                    return Redirect("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
            }

            // Invalid - redisplay with errors

            var viewModel = new StoreManagerViewModel()
            {
                Album = album,
                Genres = new SelectList(storeDB.Genres.ToList(), "GenreId", "Name", album.GenreId),
                Artists = new SelectList(storeDB.Artists.ToList(), "ArtistId", "Name", album.ArtistId)
            };

            return View(viewModel);
        }
        
        public ActionResult Edit(int id)
        {
            Album album = storeDB.Albums.Single(a => a.AlbumId == id);
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var album = storeDB.Albums.Single(a => a.AlbumId == id);

            try
            {
                // Save Album
                UpdateModel(album, "Album");
                storeDB.SaveChanges();
 
                return RedirectToAction("Index");
            }
            catch
            {
                // Error ocurred - so redisplay the for
                ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.GenreId);
                ViewBag.ArtistId = new SelectList(storeDB.Artists, "ArtistId", "Name", album.ArtistId);
                return View(album);
            }
        }

        public ActionResult Delete(int id)
        {
            var album = storeDB.Albums.Single(a => a.AlbumId == id);
            return View(album);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var album = storeDB.Albums
                .Include("OrderDetails").Include("Carts")
                .Single(a => a.AlbumId == id);

            storeDB.DeleteObject(album);
            storeDB.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
