using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eBae_MVC.Models
{
    public class AuctionInfo
    {
        public int Leader { get; set; }
        public int Outbid { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }

    }
}