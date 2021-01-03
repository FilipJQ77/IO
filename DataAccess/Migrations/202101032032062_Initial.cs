namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        NumberOfEcts = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        CourseType = c.Int(nullable: false),
                        CourseGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseGroups", t => t.CourseGroupId, cascadeDelete: true)
                .Index(t => t.CourseGroupId);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tutor = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Classroom = c.String(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.StudentDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Semester = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentDataLessons",
                c => new
                    {
                        StudentData_Id = c.Int(nullable: false),
                        Lesson_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentData_Id, t.Lesson_Id })
                .ForeignKey("dbo.StudentDatas", t => t.StudentData_Id, cascadeDelete: false)
                .ForeignKey("dbo.Lessons", t => t.Lesson_Id, cascadeDelete: false)
                .Index(t => t.StudentData_Id)
                .Index(t => t.Lesson_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentDataLessons", "Lesson_Id", "dbo.Lessons");
            DropForeignKey("dbo.StudentDataLessons", "StudentData_Id", "dbo.StudentDatas");
            DropForeignKey("dbo.StudentDatas", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.CourseGroups", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Lessons", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CourseGroupId", "dbo.CourseGroups");
            DropIndex("dbo.StudentDataLessons", new[] { "Lesson_Id" });
            DropIndex("dbo.StudentDataLessons", new[] { "StudentData_Id" });
            DropIndex("dbo.StudentDatas", new[] { "FieldId" });
            DropIndex("dbo.Lessons", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "CourseGroupId" });
            DropIndex("dbo.CourseGroups", new[] { "FieldId" });
            DropTable("dbo.StudentDataLessons");
            DropTable("dbo.Users");
            DropTable("dbo.Fields");
            DropTable("dbo.StudentDatas");
            DropTable("dbo.Lessons");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseGroups");
        }
    }
}
