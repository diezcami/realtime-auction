using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBae_MVC.Models;
using eBae_MVC.DAL;

namespace eBae_MVC.Controllers
{
    public class StoreController : Controller
    {
        private AuctionContext db = new AuctionContext();
        //
        // GET: /Store/

        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Store/Browse
        public ActionResult Browse()
        {
            return View();
        }
        //
        // GET: /Store/Details
        public ActionResult Details(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

    }
}
