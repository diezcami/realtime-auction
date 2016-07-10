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
            Session["CurrentListingID"] = id;
            ViewBag.Image = Url.Content("~/Content/Images/" + id.ToString() + ".jpg");
            ViewBag.DaysRemaining = (listing.EndTimestamp - DateTime.Now).Days;
            ViewBag.HoursRemaining = (listing.EndTimestamp - DateTime.Now).Hours;
            ViewBag.MinutesRemaining = (listing.EndTimestamp - DateTime.Now).Minutes;
            ViewBag.SecondsRemaining = (listing.EndTimestamp - DateTime.Now).Seconds;
            return View(listing);
        }

        //
        // POST: /Listing/Details/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Bid bid)
        {
            int currentListingID = Convert.ToInt32(Session["CurrentListingID"]);
            Listing currentListingOwner = db.Listings.FirstOrDefault(l => l.ListingID == currentListingID);
            int currentListingOwnerID = currentListingOwner.UserID;

            // Check if the link is valid
            if (ModelState.IsValid)
            {
                // Check if the user's different from the listing owner
                if (currentListingOwnerID != Convert.ToInt32(Session["CurrentUserID"]))
                {
                    // Check for consecutive bids
                    Listing listing = db.Listings.Find(currentListingID);
                    // Dirty workaround
                    Bid latestBid = null;

                    foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                        latestBid = b;

                    // Can't bid on finished auctions                   
                    if (listing.EndTimestamp.Subtract(DateTime.Now).Seconds > 0) {
                        if (latestBid == null || latestBid.UserID != Convert.ToInt32(Session["CurrentUserID"])) {
                            bid.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                            bid.User = db.Users.FirstOrDefault(u => u.UserID == bid.UserID);
                            bid.ListingID = Convert.ToInt32(Session["CurrentListingID"]);
                            bid.Listing = db.Listings.FirstOrDefault(l => l.ListingID == bid.ListingID);
                            bid.Timestamp = DateTime.Now;

                            db.Bids.Add(bid);
                            db.SaveChanges();
                    
                            return RedirectToAction("Details");
                        } 
                        else 
                        {
                            return RedirectToAction("Error", new { ErrorID = 4 });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error", new { ErrorID = 3 });
                    }
                }
                else
                {
                    return RedirectToAction("Error", new { ErrorID = 2 });
                }
            }
            return RedirectToAction("Error", new { ErrorID = 1 });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Error(int ErrorID)
        {
            switch (ErrorID)
            {
                case 1:
                    ViewBag.ErrorText = "Invalid input. Please try again.";
                    break;
                case 2:
                    ViewBag.ErrorText = "You cannot bid on your own listing.";
                    break;
                case 3:
                    ViewBag.ErrorText = "This auction has already ended.";
                    break;
                case 4:
                    ViewBag.ErrorText = "You cannot bid on the same listing twice in a row.";
                    break;
                default:
                    ViewBag.ErrorText = "Something went wrong.";
                    break;
            }
            return View();
        }
    }
}