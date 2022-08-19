using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AniChan8.Models;

namespace AniChan8.Controllers
{
    public class HomeController : Controller
    {
        AniChanEntities db = new AniChanEntities();
        public ActionResult Index()
        {
            List<Anime> animeList = db.Animes.SqlQuery("select top 8 * from Anime order by rating desc").ToList();
            
            return View(animeList);
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
