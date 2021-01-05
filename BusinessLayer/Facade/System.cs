using DataAccess.BusinessObjects.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Facade
{
    using Controllers;
    class System : ISystem
    {
        public (bool, string) AddAccount(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AddCourse(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AddCoursesGroup(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AddField(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AddLesson(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) AssignRegistrationDate(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) EditAccount(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) EditCourse(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) EditCoursesGroup(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) EditField(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) EditLesson(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) LogIn(Dictionary<string, string> data)
        {
            return new UserController().LogIn(data);
        }

        public void LogOut(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> ShowAccounts(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> ShowCourses(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CourseGroup> ShowCoursesGroups(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ShowErrors(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Field> ShowFields(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lesson> ShowLessons(string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lesson> ShowSignedLessons(string token)
        {
            throw new NotImplementedException();
        }

        public (bool, string) SignToLesson(Dictionary<string, string> data, string token)
        {
            throw new NotImplementedException();
        }
    }
}
