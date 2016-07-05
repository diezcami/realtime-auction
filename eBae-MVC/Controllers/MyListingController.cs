﻿using System;
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
            ViewBag.ImageURL = Url.Content("~/Content/Images/");
            ViewBag.JPG = ".jpg";
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
        public ActionResult Create(Listing listing)
        {
            if (ModelState.IsValid)
            {
                listing.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                listing.User = db.Users.FirstOrDefault(u => u.UserID == listing.UserID);
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