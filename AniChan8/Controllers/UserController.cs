using AniChan8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        AniChanEntities1 db = new AniChanEntities1();
        public ActionResult Bookmark()
        {
            var bookMarkList = db.Bookmarks.SqlQuery("select * from Bookmark where user_id_=@user", new SqlParameter("@user", Session["user_id_SS"])).ToList();
            List <Anime> animeList = new List<Anime>();
            foreach(var entry in bookMarkList)
            {
                var anime = db.Animes.SqlQuery("select * from Anime where anime_id=@anime", new SqlParameter("@anime", entry.anime_id)).FirstOrDefault();
                animeList.Add(anime);
            }
            return View(animeList);
        }
    }
}
