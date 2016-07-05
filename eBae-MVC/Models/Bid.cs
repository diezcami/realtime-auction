using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eBae_MVC.Models
{
    public class Bid
    {
        public int BidID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }

        public DateTime Timestamp { get; set; }
        public int Amount { get; set; }

        public virtual User User { get; set; }
        public virtual Listing Listing { get; set; }

    }
}