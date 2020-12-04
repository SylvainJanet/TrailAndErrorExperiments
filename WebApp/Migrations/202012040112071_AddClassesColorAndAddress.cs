namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassesColorAndAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Number = c.Int(nullable: false),
                        Street = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => new { t.Number, t.Street });
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.People", "Address_Number", c => c.Int());
            AddColumn("dbo.People", "Address_Street", c => c.String(maxLength: 200));
            AddColumn("dbo.People", "FavoriteColor_Id", c => c.Int());
            AddColumn("dbo.People", "LeastLikedColor_Id", c => c.Int());
            CreateIndex("dbo.People", new[] { "Address_Number", "Address_Street" });
            CreateIndex("dbo.People", "FavoriteColor_Id");
            CreateIndex("dbo.People", "LeastLikedColor_Id");
            AddForeignKey("dbo.People", new[] { "Address_Number", "Address_Street" }, "dbo.Addresses", new[] { "Number", "Street" });
            AddForeignKey("dbo.People", "FavoriteColor_Id", "dbo.Colors", "Id");
            AddForeignKey("dbo.People", "LeastLikedColor_Id", "dbo.Colors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "LeastLikedColor_Id", "dbo.Colors");
            DropForeignKey("dbo.People", "FavoriteColor_Id", "dbo.Colors");
            DropForeignKey("dbo.People", new[] { "Address_Number", "Address_Street" }, "dbo.Addresses");
            DropIndex("dbo.People", new[] { "LeastLikedColor_Id" });
            DropIndex("dbo.People", new[] { "FavoriteColor_Id" });
            DropIndex("dbo.People", new[] { "Address_Number", "Address_Street" });
            DropColumn("dbo.People", "LeastLikedColor_Id");
            DropColumn("dbo.People", "FavoriteColor_Id");
            DropColumn("dbo.People", "Address_Street");
            DropColumn("dbo.People", "Address_Number");
            DropTable("dbo.Colors");
            DropTable("dbo.Addresses");
        }
    }
}
