namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressIsBaseEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Id");
        }
    }
}
