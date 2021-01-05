namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentDatas", "RegistrationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentDatas", "RegistrationDate", c => c.DateTime(nullable: false));
        }
    }
}
