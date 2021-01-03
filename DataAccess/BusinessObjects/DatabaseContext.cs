using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BusinessObjects.Entities;

namespace DataAccess.BusinessObjects
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() :base("ConnectionString")
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseGroup> CourseGroups { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<StudentData> StudentDatas { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
