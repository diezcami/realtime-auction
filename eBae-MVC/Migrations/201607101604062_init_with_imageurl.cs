namespace eBae_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_with_imageurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listing", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listing", "ImageUrl");
        }
    }
}
