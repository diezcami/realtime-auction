using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eBae_MVC.Models
{
    public class Listing
    {
        public int ListingID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        
        public string ImageUrl { get; set; }
        public int StartingPrice { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}