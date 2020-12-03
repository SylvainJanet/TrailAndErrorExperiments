namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedThought : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Thoughts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonThoughtComfy",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        ThoughtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ThoughtId })
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: false)
                .ForeignKey("dbo.Thoughts", t => t.ThoughtId, cascadeDelete: false)
                .Index(t => t.PersonId)
                .Index(t => t.ThoughtId);
            
            CreateTable(
                "dbo.PersonThoughtTimid",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        ThoughtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ThoughtId })
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: false)
                .ForeignKey("dbo.Thoughts", t => t.ThoughtId, cascadeDelete: false)
                .Index(t => t.PersonId)
                .Index(t => t.ThoughtId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonThoughtTimid", "ThoughtId", "dbo.Thoughts");
            DropForeignKey("dbo.PersonThoughtTimid", "PersonId", "dbo.People");
            DropForeignKey("dbo.PersonThoughtComfy", "ThoughtId", "dbo.Thoughts");
            DropForeignKey("dbo.PersonThoughtComfy", "PersonId", "dbo.People");
            DropIndex("dbo.PersonThoughtTimid", new[] { "ThoughtId" });
            DropIndex("dbo.PersonThoughtTimid", new[] { "PersonId" });
            DropIndex("dbo.PersonThoughtComfy", new[] { "ThoughtId" });
            DropIndex("dbo.PersonThoughtComfy", new[] { "PersonId" });
            DropTable("dbo.PersonThoughtTimid");
            DropTable("dbo.PersonThoughtComfy");
            DropTable("dbo.Thoughts");
        }
    }
}
