// <auto-generated/>
namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConditionsOfSatisfaction = c.String(),
                        Summary = c.String(),
                        Description = c.String(),
                        HoursRemaining = c.Double(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Assignee_Id = c.Guid(),
                        Story_Id = c.Guid(),
                        Tester_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Assignee_Id)
                .ForeignKey("dbo.Stories", t => t.Story_Id)
                .ForeignKey("dbo.Users", t => t.Tester_Id)
                .Index(t => t.Assignee_Id)
                .Index(t => t.Story_Id)
                .Index(t => t.Tester_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Tester_Id", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Story_Id", "dbo.Stories");
            DropForeignKey("dbo.Tasks", "Assignee_Id", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "Tester_Id" });
            DropIndex("dbo.Tasks", new[] { "Story_Id" });
            DropIndex("dbo.Tasks", new[] { "Assignee_Id" });
            DropTable("dbo.Tasks");
        }
    }
}