namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.StudentDatas", "RegistrationDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }

        public override void Down()
        {
            AlterColumn("dbo.StudentDatas", "RegistrationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(nullable: false));
        }
    }
}
