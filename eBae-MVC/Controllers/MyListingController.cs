using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBae_MVC.Models;
using eBae_MVC.DAL;

namespace eBae_MVC.Controllers
{
    public class MyListingController : Controller
    {
        private AuctionContext db = new AuctionContext();

        //
        // GET: /MyListing/

        public ActionResult Index()
        {
            string currentUser = Session["CurrentUsername"].ToString();
            var listings = db.Listings.Include(l => l.User).Where (l => l.User.Username.Equals(currentUser));
            return View(listings.ToList());
        }

        //
        // GET: /MyListing/Details/5

        public ActionResult Details(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //
        // GET: /MyListing/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MyListing/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Listing listing, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                listing.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                listing.User = db.Users.FirstOrDefault(u => u.UserID == listing.UserID);
                listing.StartTimestamp = DateTime.Now;
                

                if (file != null)
                {
                    string FileName = System.IO.Path.GetFileName(file.FileName);
                    string FileExtension = FileName.Substring(FileName.IndexOf("."));
                    string FinalFileName = listing.Title + FileExtension;
                    string Path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images"), FinalFileName);
                    file.SaveAs(Path);
                    listing.ImageUrl = Url.Content("~/Content/Images/") + FinalFileName;
                }

                db.Listings.Add(listing);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", listing.UserID);
            return View(listing);
        }

        //
        // GET: /MyListing/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", listing.UserID);
            return View(listing);
        }

        //
        // POST: /MyListing/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", listing.UserID);
            return View(listing);
        }

        //
        // GET: /MyListing/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //
        // POST: /MyListing/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}