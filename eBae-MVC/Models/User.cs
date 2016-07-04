using System;
using System.Collections.Generic;

namespace eBae_MVC.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Watch> Watches { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}