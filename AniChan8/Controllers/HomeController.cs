using AniChan8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class HomeController : Controller
    {
        AniChanEntities1 db = new AniChanEntities1();
        //[HttpPost]
        public ActionResult Index()
        {
            List<Anime> animeList = db.Animes.SqlQuery("select top 6 * from Anime order by rating desc").ToList();
            List<Anime> newest = db.Animes.SqlQuery("select top 6 * from Anime order by upload_date desc").ToList();
            ViewBag.newest = newest;
            return View(animeList);
        }

        [HttpPost]
        public ActionResult Search(string keyword)
        {
            if(keyword == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.key = keyword;
            return View(db.Animes.Where(x => x.title.Contains(keyword) || keyword == null).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Credits()
        {
            return View();
        }
    }
}
