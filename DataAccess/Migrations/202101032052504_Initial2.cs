namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentDatas", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.StudentDatas", "UserId");
            AddForeignKey("dbo.StudentDatas", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.StudentDatas", "UserId", "dbo.Users");
            DropIndex("dbo.StudentDatas", new[] { "UserId" });
            DropColumn("dbo.StudentDatas", "UserId");
        }
    }
}
