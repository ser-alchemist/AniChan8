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
            if(anime == null || episode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Anime animeInfo = db.Animes.SqlQuery("select top 1 * from Anime where anime_id=@anime", new SqlParameter("@anime", anime)).FirstOrDefault();
            Episode episodeInfo = db.Episodes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode", new SqlParameter("@anime", anime), new SqlParameter("@episode", episode)).FirstOrDefault();
            //List<Anime> episodeInfo = db.Animes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode").ToList();
            
            if(Session != null && Session["user_id_SS"] != null)
            {
                ViewBag.bmBtn = true;
                ViewBag.rateBtn = true;
                var isBookmarked = db.Bookmarks.SqlQuery("select top 1 * from Bookmark where anime_id=@anime and user_id_=@user", new SqlParameter("@anime", anime), new SqlParameter("@user", Session["user_id_SS"])).FirstOrDefault();
                var isRated = db.Ratings.SqlQuery("select top 1 * from Rating where anime_id=@anime and user_id_=@user", new SqlParameter("@anime", anime), new SqlParameter("@user", Session["user_id_SS"])).FirstOrDefault();
                if (isBookmarked != null)
                {
                    ViewBag.isBm = true;

                }
                else
                {
                    ViewBag.isBm = false;
                }

                if(isRated != null)
                {
                    ViewBag.isRt = true;
                    ViewBag.currentRating = Convert.ToInt32(isRated.rating);
                }
                else
                {
                    ViewBag.isRt = false;
                }
            }
            else
            {
                ViewBag.bmBtn = false;
                ViewBag.rateBtn = false;
            }

            ViewBag.Anime = animeInfo;
            ViewBag.Episode = episodeInfo;
            ViewBag.currentEp = episode;
            
            return View();
        }

        [HttpPost]
        public ActionResult AddBookmark(int? anime, int? episode)
        {
            if (anime == null || episode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string query = String.Format("insert into Bookmark(user_id_, anime_id) values ({0}, {1})", Session["user_id_SS"], anime);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Play", new { anime = anime, episode = episode });
            
        }

        [HttpPost]
        public ActionResult RemoveBookmark(int? anime, int? episode)
        {
            if (anime == null || episode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string query = String.Format("delete from Bookmark where anime_id={0} and user_id_={1}", anime, Session["user_id_SS"]);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Play", new { anime = anime, episode = episode });
        }

        [HttpPost]
        public ActionResult AddRating(int? anime, int? episode, int? rate)
        {
            if (anime == null || episode == null || rate == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string query = String.Format("insert into Rating(user_id_, anime_id, rating) values ({0}, {1}, {2})", Session["user_id_SS"], anime, rate);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            //var r = db.Ratings.SqlQuery("select AVG(rating) as avgRate from Rating where anime_id=@anime", new SqlParameter("@anime", anime)).FirstOrDefault();
            var avgRating = (from rt in db.Ratings.Where(x => x.anime_id == anime)
                             select rt.rating).Average();
            //string queryR = String.Format("update Anime set rating = {0} where anime_id = {1}", avgRating, anime);
            //int noOfRowInsertedR = db.Database.ExecuteSqlCommand(query);
            (from a in db.Animes where a.anime_id == anime select a).ToList().ForEach(x => x.rating = avgRating);
            db.SaveChanges();
            return RedirectToAction("Play", new { anime = anime, episode = episode });

        }

        [HttpPost]
        public ActionResult UpdateRating(int? anime, int? episode, int? rate)
        {
            if (anime == null || episode == null || rate == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string query = String.Format("update Rating set rating = {0} where anime_id = {1} and user_id_ = {2}", rate, anime, Session["user_id_SS"]);
            int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
            var avgRating = (from rt in db.Ratings.Where(x => x.anime_id == anime)
                             select rt.rating).Average();
            //string queryR = String.Format("update Anime set rating = {0} where anime_id = {1}", avgRating, anime);
            //int noOfRowInsertedR = db.Database.ExecuteSqlCommand(query);
            (from a in db.Animes where a.anime_id == anime select a).ToList().ForEach(x => x.rating = avgRating);
            db.SaveChanges();
            return RedirectToAction("Play", new { anime = anime, episode = episode });

        }
    }
}
