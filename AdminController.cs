using AniChan8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class AdminController : Controller
    {
        AniChanEntities db = new AniChanEntities();
        //[HttpPost]
        public ActionResult Index()
        {
            return View(db.Anime.ToList());
        }
        
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create(Anime anime)
        {
            try
            {
                db.Anime.Add(anime);
                db.SaveChanges();

                return RedirecToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}