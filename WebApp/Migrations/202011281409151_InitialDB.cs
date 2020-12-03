namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brains",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fingers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonIdea",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        IdeaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.IdeaId })
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: false)
                .ForeignKey("dbo.Ideas", t => t.IdeaId, cascadeDelete: false)
                .Index(t => t.PersonId)
                .Index(t => t.IdeaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonIdea", "IdeaId", "dbo.Ideas");
            DropForeignKey("dbo.PersonIdea", "PersonId", "dbo.People");
            DropForeignKey("dbo.Fingers", "OwnerId", "dbo.People");
            DropForeignKey("dbo.Brains", "Id", "dbo.People");
            DropIndex("dbo.PersonIdea", new[] { "IdeaId" });
            DropIndex("dbo.PersonIdea", new[] { "PersonId" });
            DropIndex("dbo.Fingers", new[] { "OwnerId" });
            DropIndex("dbo.Brains", new[] { "Id" });
            DropTable("dbo.PersonIdea");
            DropTable("dbo.Ideas");
            DropTable("dbo.Fingers");
            DropTable("dbo.People");
            DropTable("dbo.Brains");
        }
    }
}
