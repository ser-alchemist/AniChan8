using AniChan8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        AniChanEntities1 db = new AniChanEntities1();

        public ActionResult Play(int? anime, int? episode)
        {
            Anime animeInfo = db.Animes.SqlQuery("select top 1 * from Anime where anime_id=@anime", new SqlParameter("@anime", anime)).FirstOrDefault();
            Episode episodeInfo = db.Episodes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode", new SqlParameter("@anime", anime), new SqlParameter("@episode", episode)).FirstOrDefault();
            //List<Anime> episodeInfo = db.Animes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode").ToList();
            
            if(Session != null && Session["user_id_SS"] != null)
            {
                ViewBag.bmBtn = true;
                var isBookmarked = db.Bookmarks.SqlQuery("select top 1 * from Bookmark where anime_id=@anime and user_id_=@user", new SqlParameter("@anime", anime), new SqlParameter("@user", Session["user_id_SS"])).FirstOrDefault();
                if (isBookmarked != null)
                {
                    ViewBag.isBm = true;

                }
                else
                {
                    ViewBag.isBm = false;
                }
            }
            else
            {
                ViewBag.bmBtn = false;
            }

            ViewBag.Anime = animeInfo;
            ViewBag.Episode = episodeInfo;
            ViewBag.currentEp = episode;
            return View();
        }

        [HttpPost]
        public ActionResult AddBookmark(int? anime, int? episode)
        {

            string query = String.Format("insert into Bookmark(user_id_, anime_id) values ({0}, {1})", Session["user_id_SS"], anime);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Play", new { anime = anime, episode = episode });
            
        }

        public ActionResult RemoveBookmark(int? anime, int? episode)
        {
            string query = String.Format("delete from Bookmark where anime_id={0} and user_id_={1}", anime, Session["user_id_SS"]);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Play", new { anime = anime, episode = episode });
        }

    }
}
