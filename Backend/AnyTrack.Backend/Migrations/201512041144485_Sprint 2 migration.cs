// <auto-generated />
namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sprint2migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        VersionControl = c.String(),
                        StartedOn = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        ProductOwner_Id = c.Guid(),
                        ProjectManager_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ProductOwner_Id)
                .ForeignKey("dbo.Users", t => t.ProjectManager_Id)
                .Index(t => t.ProductOwner_Id)
                .Index(t => t.ProjectManager_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EmailAddress = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ScrumMaster = c.Boolean(nullable: false),
                        ProductOwner = c.Boolean(nullable: false),
                        Developer = c.Boolean(nullable: false),
                        SecretQuestion = c.String(),
                        SecretAnswer = c.String(),
                        Skills = c.String(),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleName = c.String(),
                        ProjectId = c.Guid(nullable: false),
                        SprintId = c.Guid(),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Sprints",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Project_Id = c.Guid(),
                        ScrumMaster_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Users", t => t.ScrumMaster_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.ScrumMaster_Id);
            
            CreateTable(
                "dbo.SprintStories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StoryEstimate = c.Double(nullable: false),
                        DateCompleted = c.DateTime(),
                        Status = c.String(),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Sprint_Id = c.Guid(),
                        Story_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sprints", t => t.Sprint_Id)
                .ForeignKey("dbo.Stories", t => t.Story_Id)
                .Index(t => t.Sprint_Id)
                .Index(t => t.Story_Id);
            
            CreateTable(
                "dbo.Stories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Summary = c.String(),
                        ConditionsOfSatisfaction = c.String(),
                        AsA = c.String(),
                        IWant = c.String(),
                        SoThat = c.String(),
                        InSprint = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Project_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConditionsOfSatisfaction = c.String(),
                        Summary = c.String(),
                        Description = c.String(),
                        Blocked = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Assignee_Id = c.Guid(),
                        SprintStory_Id = c.Guid(),
                        Tester_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Assignee_Id)
                .ForeignKey("dbo.SprintStories", t => t.SprintStory_Id)
                .ForeignKey("dbo.Users", t => t.Tester_Id)
                .Index(t => t.Assignee_Id)
                .Index(t => t.SprintStory_Id)
                .Index(t => t.Tester_Id);
            
            CreateTable(
                "dbo.TaskHourEstimates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Estimate = c.Double(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Project_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SprintUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SprintUsers", "Sprint_Id", "dbo.Sprints");
            DropForeignKey("dbo.Sprints", "ScrumMaster_Id", "dbo.Users");
            DropForeignKey("dbo.Sprints", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "Tester_Id", "dbo.Users");
            DropForeignKey("dbo.TaskHourEstimates", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "SprintStory_Id", "dbo.SprintStories");
            DropForeignKey("dbo.Tasks", "Assignee_Id", "dbo.Users");
            DropForeignKey("dbo.SprintStories", "Story_Id", "dbo.Stories");
            DropForeignKey("dbo.Stories", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.SprintStories", "Sprint_Id", "dbo.Sprints");
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectManager_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "ProductOwner_Id", "dbo.Users");
            DropForeignKey("dbo.Roles", "User_Id", "dbo.Users");
            DropIndex("dbo.SprintUsers", new[] { "User_Id" });
            DropIndex("dbo.SprintUsers", new[] { "Sprint_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            DropIndex("dbo.TaskHourEstimates", new[] { "Task_Id" });
            DropIndex("dbo.Tasks", new[] { "Tester_Id" });
            DropIndex("dbo.Tasks", new[] { "SprintStory_Id" });
            DropIndex("dbo.Tasks", new[] { "Assignee_Id" });
            DropIndex("dbo.Stories", new[] { "Project_Id" });
            DropIndex("dbo.SprintStories", new[] { "Story_Id" });
            DropIndex("dbo.SprintStories", new[] { "Sprint_Id" });
            DropIndex("dbo.Sprints", new[] { "ScrumMaster_Id" });
            DropIndex("dbo.Sprints", new[] { "Project_Id" });
            DropIndex("dbo.Roles", new[] { "User_Id" });
            DropIndex("dbo.Projects", new[] { "ProjectManager_Id" });
            DropIndex("dbo.Projects", new[] { "ProductOwner_Id" });
            DropTable("dbo.SprintUsers");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.TaskHourEstimates");
            DropTable("dbo.Tasks");
            DropTable("dbo.Stories");
            DropTable("dbo.SprintStories");
            DropTable("dbo.Sprints");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
        }
    }
}