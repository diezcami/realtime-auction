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

        protected void Application_Start()
        {
            HttpContext context = HttpContext.Current;

            AddTask("DoStuff", 10);

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
            AddTask(k, Convert.ToInt32(v));
        }
    }


}