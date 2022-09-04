using AniChan8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AniChan8.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        AniChanEntities1 db = new AniChanEntities1();


        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ViewBag.showError = false;
            if ( String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) )
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "All feilds are reqiured!";
                return View();

            }
            string qry1 = String.Format("select top 1 * from Users where user_name_='{0}'", username);
            var user1 = db.Users.SqlQuery(qry1).FirstOrDefault();
            
            if(user1 is null)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "Username doesn't exist!";
                return View();
            }
            else
            {
                if(user1.password_ != password)
                {
                    ViewBag.showError = true;
                    ViewBag.errorMessage = "Password doesn't match!";
                    return View();
                }
                else
                {
                    ViewBag.showError = false;
                    Session["user_id_SS"] = user1.user_id_.ToString();
                    Session["user_name_SS"] = user1.user_name_.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
            
        }

        public ActionResult Login(int? message)
        {
            if (message == 1)
            {
                ViewBag.showMessage = true;
                ViewBag.message = "Registration Successfull! Login to continue...";
            }
            else if(message == 2)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "You need to login to watch the content you have requested!";
            }
            else
            {
                ViewBag.showMessage = false;
            }
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string email, string user_name, string password)
        {
            ViewBag.showError = false;
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(user_name) || String.IsNullOrEmpty(password))
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "All feilds are reqiured!";
                return View();

            }
            //string uname = db.Users.SqlQuery<string>("select top 1 user_id_ from Users where user_name_='rafi'").FirstOrDefault();
            //var user1 = db.Users.SqlQuery("select top 1 * from Users where user_name_='@uname'", new SqlParameter("@uname", user_name)).ToList();
            //var user2 = db.Users.SqlQuery("select top 1 * from Users where email='@email'", new SqlParameter("@email", email)).ToList();
            string qry1 = String.Format("select top 1 * from Users where email='{0}'", email);
            var user1 = db.Users.SqlQuery(qry1).FirstOrDefault();
            string qry2 = String.Format("select top 1 * from Users where user_name_='{0}'", user_name);
            var user2 = db.Users.SqlQuery(qry2).FirstOrDefault();

            if (user1 != null)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "Email already exist!";
                return View();
            }
            else if (user2 != null)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "Username already exist!";
                return View();
            }

            /*
            if(user2.Count==0)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "Email already exist!";
                return View();
            }
            else if (user1.Count==0)
            {
                ViewBag.showError = true;
                ViewBag.errorMessage = "Username already exist!";
                return View();
            }*/
            else
            {
                /*
                var sql = @"insert into Users Users(email, user_name_, password_) values ('@email', '@uname', '@password')";

                int noOfRowInserted = db.Database.ExecuteSqlCommand(sql,
                                        new SqlParameter("@email", email),
                                        new SqlParameter("@uname", user_name),
                                        new SqlParameter("@password", password));

                
                //int noOfRowInserted = db.Database.ExecuteSqlCommand("insert into Users(email, user_name_, password_) values ('@email', '@uname', '@password')", new SqlParameter("@email", email), new SqlParameter("@uname", user_name), new SqlParameter("@password", password) );
                */
                ViewBag.showError = false;
                string query = String.Format("insert into Users(email, user_name_, password_) values ('{0}', '{1}', '{2}')", email, user_name, password);
                int noOfRowInserted = db.Database.ExecuteSqlCommand(query);
                if (noOfRowInserted > 0)
                {
                    return RedirectToAction("Login", new { message = 1 });

                }
                else
                {
                    ViewBag.showError = true;
                    ViewBag.errorMessage = "Signup NOT successfull! Please try again!";
                    return View();
                }
            }
            //return View();

        }





    }
}
