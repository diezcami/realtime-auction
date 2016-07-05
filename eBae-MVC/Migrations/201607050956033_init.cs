namespace eBae_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bid",
                c => new
                    {
                        BidID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ListingID = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BidID)
                .ForeignKey("dbo.Listing", t => t.ListingID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ListingID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Watch",
                c => new
                    {
                        WatchID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ListingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WatchID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Listing", t => t.ListingID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ListingID);
            
            CreateTable(
                "dbo.Listing",
                c => new
                    {
                        ListingID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        StartTimestamp = c.DateTime(nullable: false),
                        EndTimestamp = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ListingID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ClosingHistory",
                c => new
                    {
                        ClosingHistoryID = c.Int(nullable: false, identity: true),
                        BidID = c.Int(nullable: false),
                        ListingID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClosingHistoryID)
                .ForeignKey("dbo.Bid", t => t.BidID, cascadeDelete: true)
                .ForeignKey("dbo.Listing", t => t.ListingID, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: false)
                .Index(t => t.BidID)
                .Index(t => t.ListingID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClosingHistory", new[] { "UserID" });
            DropIndex("dbo.ClosingHistory", new[] { "ListingID" });
            DropIndex("dbo.ClosingHistory", new[] { "BidID" });
            DropIndex("dbo.Listing", new[] { "UserID" });
            DropIndex("dbo.Watch", new[] { "ListingID" });
            DropIndex("dbo.Watch", new[] { "UserID" });
            DropIndex("dbo.Bid", new[] { "UserID" });
            DropIndex("dbo.Bid", new[] { "ListingID" });
            DropForeignKey("dbo.ClosingHistory", "UserID", "dbo.User");
            DropForeignKey("dbo.ClosingHistory", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.ClosingHistory", "BidID", "dbo.Bid");
            DropForeignKey("dbo.Listing", "UserID", "dbo.User");
            DropForeignKey("dbo.Watch", "ListingID", "dbo.Listing");
            DropForeignKey("dbo.Watch", "UserID", "dbo.User");
            DropForeignKey("dbo.Bid", "UserID", "dbo.User");
            DropForeignKey("dbo.Bid", "ListingID", "dbo.Listing");
            DropTable("dbo.ClosingHistory");
            DropTable("dbo.Listing");
            DropTable("dbo.Watch");
            DropTable("dbo.User");
            DropTable("dbo.Bid");
        }
    }
}
