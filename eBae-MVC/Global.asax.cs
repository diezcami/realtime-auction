using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eBae_MVC.Models;
using eBae_MVC.DAL;
using System.Data.Objects;

namespace eBae_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;
        AuctionContext db = new AuctionContext();
        HttpContext context;

        protected void Application_Start()
        {
            context = HttpContext.Current;

            AddTask("RefreshBids", 10);
            //AddTask("RefreshNumberBar", 5);

            if (context != null && context.Session != null)
            {
                Session["CurrentUsername"] = "";
                Session["CurrentUserID"] = 0;
                Session["Leader"] = 0;
                Session["Outbid"] = 0;
                Session["Won"] = 0;
                Session["Lost"] = 0;
                
            }
            
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);        
        }

        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            if (k.Equals("RefreshBids"))
            {
                // do stuff here if it matches our taskname, like WebRequest
                // re-add our task so it recurs 
                var finishedListings = db.Listings.Where(l => EntityFunctions.DiffSeconds(l.EndTimestamp, DateTime.Now) > 0);
                foreach (var listing in finishedListings)
                {
                    // If there are no closing histories found for the listing
                    // Only create closing histories for "won" auctions
                    if (!db.ClosingHistories.Any(ch => ch.ListingID == listing.ListingID) && listing.Bids.Count > 0)
                    {
                        // Get the winning bid (dirty workaround)
                        Bid winningBid = null;
                        foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                            winningBid = b;

                        // Save data to closing history
                        ClosingHistory ch = new ClosingHistory();
                        ch.BidID = winningBid.BidID;
                        ch.Bid = winningBid;
                        ch.ListingID = listing.ListingID;
                        ch.Listing = listing;
                        ch.UserID = winningBid.UserID;
                        ch.User = winningBid.User;

                        // Save closing history to db
                        db.ClosingHistories.Add(ch);

                    }
                }

                db.SaveChanges();
            } 
            else if (k.Equals("RefreshNumberBar"))
            {
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

                Session["Leader"] = leader;
                Session["Outbid"] = listingCount - leader;

                // Finished listings won in
                Session["Won"] = db.ClosingHistories.Where(ch => ch.User.UserID == userID).Count();

                // Closing histories users don't own and lost in
                var relevantHistories = db.ClosingHistories.Where(ch => ch.UserID != userID && ch.Listing.UserID != userID);
                // Finished listings where user placed bids
                var lost = relevantHistories.Select(ch => ch.Listing).Where(l => l.Bids.Any(b => b.UserID == userID)).Count();
                Session["Lost"] = lost;
            }
            
            AddTask(k, Convert.ToInt32(v));
        }
    }


}