
namespace eBae_MVC.Models
{
    public class Watch
    {
        public int WatchID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public virtual User User { get; set; }
        public virtual Listing Listing { get; set; }
    }
}