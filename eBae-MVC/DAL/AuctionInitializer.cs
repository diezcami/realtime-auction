using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using eBae_MVC.Models;
using eBae_MVC.DAL;
using System;
using System.Data.Entity.Migrations;

namespace ContosoUniversity.DAL
{
    public class AuctionInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AuctionContext>
    {
        protected override void Seed(eBae_MVC.DAL.AuctionContext context)
        {
            var users = new List<User>
            {
                new User { Username = "Cami",   Password = "password"},
                new User { Username = "EJ",     Password = "password"},
                new User { Username = "Nina",   Password = "password"},
                new User { Username = "a",   Password = "a"},

            };
            users.ForEach(s => context.Users.AddOrUpdate(p => p.Username, s));
            context.SaveChanges();

            var listings = new List<Listing>
            {
                new Listing { 
                    Title = "Car",
                    Description = "This is a car.", 
                    StartTimestamp = DateTime.Parse("2016-06-27 13:00"), 
                    EndTimestamp = DateTime.Parse("2016-07-27 13:00"),
                    UserID = users.Single(s => s.Username == "Cami").UserID
                },
                new Listing { 
                    Title = "Piano",
                    Description = "This is a piano.", 
                    StartTimestamp = DateTime.Parse("2016-06-23 13:00"), 
                    EndTimestamp = DateTime.Parse("2016-06-30 13:00"),
                    UserID = users.Single(s => s.Username == "Nina").UserID
                },

                new Listing { 
                    Title = "Pencil",
                    Description = "This is a pencil.", 
                    StartTimestamp = DateTime.Parse("2016-06-25 13:00"), 
                    EndTimestamp = DateTime.Parse("2016-06-20 13:00"),
                    UserID = users.Single(s => s.Username == "EJ").UserID
                }
            };

            var watches = new List<Watch>
            {
                new Watch { 
                    UserID = users.Single(s => s.Username == "Cami").UserID,
                    ListingID = listings.Single(s => s.Title == "Piano").ListingID
                },
                new Watch { 
                    UserID = users.Single(s => s.Username == "Nina").UserID,
                    ListingID = listings.Single(s => s.Title == "Car").ListingID
                }
            };
          
            foreach (Watch w in watches)
            {
                var watchesInDatabase = context.Watches.Where(
                    s =>
                         s.User.UserID == w.UserID &&
                         s.Listing.ListingID == w.ListingID).SingleOrDefault();
                if (watchesInDatabase == null)
                {
                    context.Watches.Add(w);
                }
            }
            context.SaveChanges();
        }
    }
}
