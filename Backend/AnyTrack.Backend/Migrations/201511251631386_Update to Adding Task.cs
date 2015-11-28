// <auto-generated/>
namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatetoAddingTask : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tasks", name: "Story_Id", newName: "SprintStory_Id");
            RenameIndex(table: "dbo.Tasks", name: "IX_Story_Id", newName: "IX_SprintStory_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tasks", name: "IX_SprintStory_Id", newName: "IX_Story_Id");
            RenameColumn(table: "dbo.Tasks", name: "SprintStory_Id", newName: "Story_Id");
        }
    }
}
