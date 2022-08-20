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
        AniChanEntities db = new AniChanEntities();

        public ActionResult Play(int? anime, int? episode)
        {
            Anime animeInfo = db.Animes.SqlQuery("select top 1 * from Anime where anime_id=@anime", new SqlParameter("@anime", anime)).FirstOrDefault();
            Episode episodeInfo = db.Episodes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode", new SqlParameter("@anime", anime), new SqlParameter("@episode", episode)).FirstOrDefault();
            //List<Anime> episodeInfo = db.Animes.SqlQuery("select top 1 * from Episodes where anime_id=@anime AND episode_num=@episode").ToList();
            ViewBag.Anime = animeInfo;
            ViewBag.Episode = episodeInfo;
            return View();
        }

        

    }
}
