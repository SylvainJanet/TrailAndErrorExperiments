namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassAction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonAction",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ActionId })
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.ActionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonAction", "ActionId", "dbo.Actions");
            DropForeignKey("dbo.PersonAction", "PersonId", "dbo.People");
            DropIndex("dbo.PersonAction", new[] { "ActionId" });
            DropIndex("dbo.PersonAction", new[] { "PersonId" });
            DropTable("dbo.PersonAction");
            DropTable("dbo.Actions");
        }
    }
}
