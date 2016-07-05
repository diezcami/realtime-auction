namespace eBae_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClosingHistory", "Bid_BidID", "dbo.Bid");
            DropIndex("dbo.ClosingHistory", new[] { "Bid_BidID" });
            RenameColumn(table: "dbo.ClosingHistory", name: "Bid_BidID", newName: "BidID");
            AlterColumn("dbo.Bid", "BidID", c => c.Int(nullable: false, identity: true));
            AddForeignKey("dbo.ClosingHistory", "BidID", "dbo.Bid", "BidID", cascadeDelete: true);
            CreateIndex("dbo.ClosingHistory", "BidID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClosingHistory", new[] { "BidID" });
            DropForeignKey("dbo.ClosingHistory", "BidID", "dbo.Bid");
            AlterColumn("dbo.Bid", "BidID", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.ClosingHistory", name: "BidID", newName: "Bid_BidID");
            CreateIndex("dbo.ClosingHistory", "Bid_BidID");
            AddForeignKey("dbo.ClosingHistory", "Bid_BidID", "dbo.Bid", "BidID");
        }
    }
}
