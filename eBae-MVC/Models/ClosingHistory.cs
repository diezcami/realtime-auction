using System;
using System.Web;

namespace eBae_MVC.Models
{
    public class ClosingHistory
    {
        public string ClosingHistoryID { get; set; }
        public int BidID { get; set; }
        public int ListingID { get; set; }
        public int UserID { get; set; }
        public virtual Bid Bid { get; set; }
        public virtual Listing Listing { get; set; }
        public virtual User User { get; set; }
    }
}