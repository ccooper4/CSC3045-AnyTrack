//<auto-generated/>
namespace AnyTrack.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSprintStoryestimatevalue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SprintStories", "StoryEstimate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SprintStories", "StoryEstimate");
        }
    }
}
