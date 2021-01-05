namespace DataAccess.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BusinessObjects.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.BusinessObjects.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccess.BusinessObjects.DatabaseContext context)
        {
            
        }
    }
}
