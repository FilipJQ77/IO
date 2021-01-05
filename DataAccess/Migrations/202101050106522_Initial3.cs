namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseGroups", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Lessons", "Space", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "Name");
        }

        public override void Down()
        {
            AddColumn("dbo.Courses", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Lessons", "Space");
            DropColumn("dbo.CourseGroups", "Name");
        }
    }
}
