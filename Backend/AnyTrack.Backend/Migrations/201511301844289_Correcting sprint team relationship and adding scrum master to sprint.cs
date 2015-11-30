// <auto-generated/>
namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correctingsprintteamrelationshipandaddingscrummastertosprint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Sprint_Id", "dbo.Sprints");
            DropIndex("dbo.Users", new[] { "Sprint_Id" });
            CreateTable(
                "dbo.SprintUsers",
                c => new
                    {
                        Sprint_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sprint_Id, t.User_Id })
                .ForeignKey("dbo.Sprints", t => t.Sprint_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Sprint_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Sprints", "ScrumMaster_Id", c => c.Guid());
            CreateIndex("dbo.Sprints", "ScrumMaster_Id");
            AddForeignKey("dbo.Sprints", "ScrumMaster_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "Sprint_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Sprint_Id", c => c.Guid());
            DropForeignKey("dbo.SprintUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SprintUsers", "Sprint_Id", "dbo.Sprints");
            DropForeignKey("dbo.Sprints", "ScrumMaster_Id", "dbo.Users");
            DropIndex("dbo.SprintUsers", new[] { "User_Id" });
            DropIndex("dbo.SprintUsers", new[] { "Sprint_Id" });
            DropIndex("dbo.Sprints", new[] { "ScrumMaster_Id" });
            DropColumn("dbo.Sprints", "ScrumMaster_Id");
            DropTable("dbo.SprintUsers");
            CreateIndex("dbo.Users", "Sprint_Id");
            AddForeignKey("dbo.Users", "Sprint_Id", "dbo.Sprints", "Id");
        }
    }
}
