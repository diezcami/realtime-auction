using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eBae_MVC.Models;
using eBae_MVC.DAL;

namespace eBae_MVC.Controllers
{
    public class ClosingHistoryController : Controller
    {
        private AuctionContext db = new AuctionContext();

        // GET: /ClosingHistory/
        public ActionResult Index()
        {
            int userID = Convert.ToInt32(Session["CurrentUserID"]);
            var closinghistories = db.ClosingHistories.Where(ch => ch.UserID == userID);
            return View(closinghistories.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
