namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassWorldVision : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorldVisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.People", "Vision_Id", c => c.Int());
            CreateIndex("dbo.People", "Vision_Id");
            AddForeignKey("dbo.People", "Vision_Id", "dbo.WorldVisions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Vision_Id", "dbo.WorldVisions");
            DropIndex("dbo.People", new[] { "Vision_Id" });
            DropColumn("dbo.People", "Vision_Id");
            DropTable("dbo.WorldVisions");
        }
    }
}
