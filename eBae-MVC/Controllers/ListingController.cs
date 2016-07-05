using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBae_MVC.Models;
using eBae_MVC.DAL;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace eBae_MVC.Controllers
{
    public class ListingController : Controller
    {
        private AuctionContext db = new AuctionContext();

        //
        // GET: /Listing/

        public ActionResult Index()
        {
            var listings = db.Listings.Include(l => l.User);
            ViewBag.ImageURL = Url.Content("~/Content/Images/");
            ViewBag.JPG = ".jpg";
            return View(listings.ToList());
        }

        //
        // GET: /Listing/Details/5

        public ActionResult Details(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            Session["CurrentListing"] = id;
            ViewBag.Image = Url.Content("~/Content/Images/" + id.ToString() + ".jpg");
            return View(listing);
        }

        //
        // POST: /Listing/Details/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Bid bid)
        {
            if (ModelState.IsValid)
            {
                bid.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                bid.User = db.Users.FirstOrDefault(u => u.UserID == bid.UserID);
                bid.ListingID = Convert.ToInt32(Session["CurrentListing"]);
                bid.Listing = db.Listings.FirstOrDefault(l => l.ListingID == bid.ListingID);
                bid.Timestamp = DateTime.Now;

                db.Bids.Add(bid);
                db.SaveChanges();
                

                return RedirectToAction("Details");
            }

            return View(bid);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}