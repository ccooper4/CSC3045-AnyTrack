namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUpdatedHours : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UpdatedHours",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NewEstimate = c.Double(nullable: false),
                        LogEstimate = c.Double(nullable: false),
                        Updated = c.DateTime(),
                        Created = c.DateTime(),
                        Task_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UpdatedHours", "Task_Id", "dbo.Tasks");
            DropIndex("dbo.UpdatedHours", new[] { "Task_Id" });
            DropTable("dbo.UpdatedHours");
        }
    }
}
