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

        [HttpPost]
        public ActionResult RefreshViews()
        {
            var ai = new AuctionInfo();

            int userID = Convert.ToInt32(Session["CurrentUserID"]);
            // Active listings user participated in
            var listings = db.Listings.Where(l => l.EndTimestamp > DateTime.Now && l.Bids.Any(b => b.UserID == userID));
            // Get all the first bids from the listings
            var listingCount = listings.Count();
            var leader = 0;
            foreach (var listing in listings)
            {
                foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                {
                    if (b.UserID == userID)
                    {
                        leader += 1;
                    }
                }
            }
            // Count bids where user's the winner

            ai.Leader = leader;
            ai.Outbid = listingCount - leader;

            // Finished listings won in
            ai.Won = db.ClosingHistories.Where(ch => ch.User.UserID == userID).Count();

            // Closing histories users don't own and lost in
            var relevantHistories = db.ClosingHistories.Where(ch => ch.UserID != userID && ch.Listing.UserID != userID);
            // Finished listings where user placed bids
            var lost = relevantHistories.Select(ch => ch.Listing).Where(l => l.Bids.Any(b => b.UserID == userID)).Count();
            ai.Lost = lost;

            return Json(ai);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
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
                    // Active listings user participated in
                    var listings = db.Listings.Where(l => l.EndTimestamp > DateTime.Now && l.Bids.Any(b => b.UserID == myUser.UserID));
                    // Get all the first bids from the listings
                    var listingCount = listings.Count();
                    var leader = 0;
                    foreach (var listing in listings)
                    {
                        foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                        {
                            if (b.UserID == myUser.UserID)
                            {
                                leader += 1;
                            }
                        }
                    }
                    // Count bids where user's the winner

                    Session["Leader"] = leader;
                    Session["Outbid"] = listingCount - leader;

                    // Finished listings won in
                    Session["Won"] = db.ClosingHistories.Where(ch => ch.User.UserID == myUser.UserID).Count();

                    // Closing histories users don't own and lost in
                    var relevantHistories = db.ClosingHistories.Where(ch => ch.UserID != myUser.UserID && ch.Listing.UserID != myUser.UserID);
                    // Finished listings where user placed bids
                    var lost = relevantHistories.Select(ch => ch.Listing).Where(l => l.Bids.Any(b => b.UserID == myUser.UserID)).Count();
                    Session["Lost"] = lost;

                    return RedirectToAction("Index", "Listing");
                }
            }
            return View();
        }
    }
}
