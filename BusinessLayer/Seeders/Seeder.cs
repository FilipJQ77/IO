using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Encryption;
using DataAccess.Repositories;
using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Seeders
{
    class Seeder
    {

        public void Run()
        {
            UserSeeder();
            FieldSeeder();
            StudentDataSeeder();
            CourseGroupSeeder();
            CoureSeeder();
            LessonSeeder();
        }

        protected void UserSeeder()
        {
            var userRep = new RepositoryFactory().GetRepository<User>();
            var hasher = new HasherFactory().GetHasher();
            userRep.Add(new User
            {
                Login = "Student1",
                Password = hasher.Hash("haslo1234"),
                Rank = Rank.Student,
            });

            userRep.Add(new User
            {
                Login = "Student2",
                Password = hasher.Hash("haslo1234"),
                Rank = Rank.Student,
            });

            userRep.Add(new User
            {
                Login = "Admin",
                Password = hasher.Hash("haslo1234"),
                Rank = Rank.Administrator,
            });

            userRep.SaveChanges();
        }

        protected void FieldSeeder()
        {
            var fieldRep = new RepositoryFactory().GetRepository<Field>();
            fieldRep.Add(new Field
            {
                Name = "Informatyka",
            });
            fieldRep.Add(new Field
            {
                Name = "Elektronika",
            });

            fieldRep.SaveChanges();
        }

        protected void StudentDataSeeder()
        {
            var sdRep = new RepositoryFactory().GetRepository<StudentData>();
            var userRep = new RepositoryFactory().GetRepository<User>();
            var fieldRep = new RepositoryFactory().GetRepository<Field>();

            var field1 = fieldRep.GetDetail(p => p.Name == "Informatyka");
            var user1 = userRep.GetDetail(p => p.Login == "Student1");
            var user2 = userRep.GetDetail(p => p.Login == "Student2");
            sdRep.Add(new StudentData
            {
                Field = field1,
                FirstName = "Janek",
                LastName = "Kowalski",
                Index = 12345,
                Semester = 1,
                User = user1,
            });

            sdRep.Add(new StudentData
            {
                Field = field1,
                FirstName = "Andrzej",
                LastName = "Nowak",
                Index = 54321,
                Semester = 1,
                User = user2,
            });

            sdRep.SaveChanges();
        }

        protected void CourseGroupSeeder()
        {
            var cgRep = new RepositoryFactory().GetRepository<CourseGroup>();
            var fieldRep = new RepositoryFactory().GetRepository<Field>();

            var field1 = fieldRep.GetDetail(p => p.Name == "Informatyka");

            cgRep.Add(new CourseGroup
            {
                Code = "INEK00001",
                Field = field1,
                NumberOfEcts = 15,
                Semester = 1,
                Name = "Podstawy programowania",
            });

            cgRep.Add(new CourseGroup
            {
                Code = "INEK00002",
                Field = field1,
                NumberOfEcts = 15,
                Semester = 1,
                Name = "Analiza matematyczna 1",
            });

            cgRep.SaveChanges();
        }

        protected void CoureSeeder()
        {
            var cRep = new RepositoryFactory().GetRepository<Course>();
            var cgRep = new RepositoryFactory().GetRepository<CourseGroup>();

            var courseGroup1 = cgRep.GetDetail(p => p.Code == "INEK00001");
            var courseGroup2 = cgRep.GetDetail(p => p.Code == "INEK00002");

            cRep.Add(new Course
            {
                Code = "INEK00001W",
                CourseGroup = courseGroup1,
                CourseType = CourseType.Lecture,
            });
            cRep.Add(new Course
            {
                Code = "INEK00001L",
                CourseGroup = courseGroup1,
                CourseType = CourseType.Laboratory,
            });
            cRep.Add(new Course
            {
                Code = "INEK00002W",
                CourseGroup = courseGroup2,
                CourseType = CourseType.Lecture,
            });
            cRep.Add(new Course
            {
                Code = "INEK00002S",
                CourseGroup = courseGroup2,
                CourseType = CourseType.Seminar,
            });

            cRep.SaveChanges();
        }

        protected void LessonSeeder()
        {
            var lessonRep = new RepositoryFactory().GetRepository<Lesson>();
            var cRep = new RepositoryFactory().GetRepository<Course>();

            var course1 = cRep.GetDetail(p => p.Code == "INEK00001W");
            var course2 = cRep.GetDetail(p => p.Code == "INEK00001L");
            var course3 = cRep.GetDetail(p => p.Code == "INEK00002W");
            var course4 = cRep.GetDetail(p => p.Code == "INEK00002S");

            lessonRep.Add(new Lesson{
                Classroom = "C1",
                Tutor = "Jan Mikołajczyk",
                Course = course1,
                Space = 1,
                Date = new DateTime(2021, 3, 1, 10, 15, 00),
            });
            lessonRep.Add(new Lesson
            {
                Classroom = "C2",
                Tutor = "Andrzej Jankowski",
                Course = course1,
                Space = 1,
                Date = new DateTime(2021, 3, 1, 10, 15, 00),
            });
            lessonRep.Add(new Lesson
            {
                Classroom = "C12",
                Tutor = "Maciej Wagon",
                Course = course2,
                Space = 2,
                Date = new DateTime(2021, 3, 1, 10, 15, 00),
            });
            lessonRep.Add(new Lesson
            {
                Classroom = "C4",
                Tutor = "Jan Mikołajczyk",
                Course = course2,
                Space = 2,
                Date = new DateTime(2021, 3, 1, 10, 15, 15),
            });
            lessonRep.Add(new Lesson
            {
                Classroom = "C5",
                Tutor = "Jan Mikołajczyk",
                Course = course3,
                Space = 1,
                Date = new DateTime(2021, 3, 1, 10, 15, 15),
            });
            lessonRep.Add(new Lesson
            {
                Classroom = "C6",
                Tutor = "Jan Mikołajczyk",
                Course = course4,
                Space = 2,
                Date = new DateTime(2021, 3, 1, 10, 15, 15),
            });

            lessonRep.SaveChanges();
        }
    }
}
