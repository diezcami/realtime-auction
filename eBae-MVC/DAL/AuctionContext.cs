using eBae_MVC.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eBae_MVC.DAL
{
    public class AuctionContext : DbContext
    {
        public DbSet<Bid> Bids { get; set; }
        public DbSet<ClosingHistory> ClosingHistories { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Watch> Watches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer<AuctionContext>(null);
        }
    }
}