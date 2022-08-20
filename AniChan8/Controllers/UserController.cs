using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Bookmark()
        {
            return View();
        }
    }
}