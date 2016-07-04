namespace eBae_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.Bid", "UserID", "dbo.User");
            DropForeignKey("dbo.Watch", "UserID", "dbo.User");
            DropForeignKey("dbo.Watch", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.Listing", "UserID", "dbo.User");
            DropForeignKey("dbo.ClosingHistory", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.ClosingHistory", "UserID", "dbo.User");
            DropIndex("dbo.Bid", new[] { "ListingID" });
            DropIndex("dbo.Bid", new[] { "UserID" });
            DropIndex("dbo.Watch", new[] { "UserID" });
            DropIndex("dbo.Watch", new[] { "ListingID" });
            DropIndex("dbo.Listing", new[] { "UserID" });
            DropIndex("dbo.ClosingHistory", new[] { "ListingID" });
            DropIndex("dbo.ClosingHistory", new[] { "UserID" });
            AddForeignKey("dbo.Bid", "ListingID", "dbo.Listing", "ListingID", cascadeDelete: true);
            AddForeignKey("dbo.Bid", "UserID", "dbo.User", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Watch", "UserID", "dbo.User", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Watch", "ListingID", "dbo.Listing", "ListingID", cascadeDelete: true);
            AddForeignKey("dbo.Listing", "UserID", "dbo.User", "UserID", cascadeDelete: false);
            AddForeignKey("dbo.ClosingHistory", "ListingID", "dbo.Listing", "ListingID", cascadeDelete: true);
            AddForeignKey("dbo.ClosingHistory", "UserID", "dbo.User", "UserID", cascadeDelete: true);
            CreateIndex("dbo.Bid", "ListingID");
            CreateIndex("dbo.Bid", "UserID");
            CreateIndex("dbo.Watch", "UserID");
            CreateIndex("dbo.Watch", "ListingID");
            CreateIndex("dbo.Listing", "UserID");
            CreateIndex("dbo.ClosingHistory", "ListingID");
            CreateIndex("dbo.ClosingHistory", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClosingHistory", new[] { "UserID" });
            DropIndex("dbo.ClosingHistory", new[] { "ListingID" });
            DropIndex("dbo.Listing", new[] { "UserID" });
            DropIndex("dbo.Watch", new[] { "ListingID" });
            DropIndex("dbo.Watch", new[] { "UserID" });
            DropIndex("dbo.Bid", new[] { "UserID" });
            DropIndex("dbo.Bid", new[] { "ListingID" });
            DropForeignKey("dbo.ClosingHistory", "UserID", "dbo.User");
            DropForeignKey("dbo.ClosingHistory", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.Listing", "UserID", "dbo.User");
            DropForeignKey("dbo.Watch", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.Watch", "UserID", "dbo.User");
            DropForeignKey("dbo.Bid", "UserID", "dbo.User");
            DropForeignKey("dbo.Bid", "ListingID", "dbo.Listing");
            CreateIndex("dbo.ClosingHistory", "UserID");
            CreateIndex("dbo.ClosingHistory", "ListingID");
            CreateIndex("dbo.Listing", "UserID");
            CreateIndex("dbo.Watch", "ListingID");
            CreateIndex("dbo.Watch", "UserID");
            CreateIndex("dbo.Bid", "UserID");
            CreateIndex("dbo.Bid", "ListingID");
            AddForeignKey("dbo.ClosingHistory", "UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.ClosingHistory", "ListingID", "dbo.Listing", "ListingID");
            AddForeignKey("dbo.Listing", "UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Watch", "ListingID", "dbo.Listing", "ListingID");
            AddForeignKey("dbo.Watch", "UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Bid", "UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Bid", "ListingID", "dbo.Listing", "ListingID");
        }
    }
}
