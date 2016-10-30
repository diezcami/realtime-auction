﻿using System;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace eBae_MVC
{
    public class AuctionHub : Hub
    {
        public void Send(string user, string amount, string timestamp, string listingID)
        {
            Clients.All.addBidToPage(user, amount, timestamp, listingID);
        }
    }
}