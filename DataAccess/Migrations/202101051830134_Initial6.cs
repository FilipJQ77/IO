﻿namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "Login", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Login" });
        }
    }
}
