using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBae_MVC.Models;
using eBae_MVC.DAL;

namespace eBae_MVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private AuctionContext db = new AuctionContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Username == user.Username && u.Password == user.Password))
                {
                    var myUser = db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                    Session["CurrentUserID"] = myUser.UserID;
                    Session["CurrentUsername"] = myUser.Username;
                    return RedirectToAction("Index", "Listing");
                }
            }
            return View();
        }
    }
}
