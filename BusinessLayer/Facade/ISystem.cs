using DataAccess.BusinessObjects.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Facade
{
    public interface ISystem
    {
        (bool, string) LogIn(Dictionary<string, string> data);
        void LogOut(string token);
        IEnumerable<CourseGroup> ShowCoursesGroups(string token);
        (bool, string) SignToLesson(Dictionary<string, string> data, string token);
        IEnumerable<Lesson> ShowLessons(string token);
        (Lesson, string) ShowLesson(Dictionary<string, string> data, string token);
        IEnumerable<Lesson> ShowSignedLessons(string token);
        IEnumerable<string> ShowErrors(string token);
        IEnumerable<Course> ShowCourses(string token);
        (Course, string) ShowCourse(Dictionary<string, string> data, string token);
        (bool, string) AddAccount(Dictionary<string, string> data, string token);
        (bool, string) EditAccount(Dictionary<string, string> data, string token);
        IEnumerable<User> ShowAccounts(string token);
        (User, string) ShowAccount(Dictionary<string, string> data, string token);
        (bool, string) AssignRegistrationDate(Dictionary<string, string> data, string token);
        IEnumerable<Field> ShowFields(string token);
        (Field, string) ShowField(Dictionary<string, string> data, string token);
        (bool, string) AddField(Dictionary<string, string> data, string token);
        (bool, string) EditField(Dictionary<string, string> data, string token);
        (bool, string) AddCoursesGroup(Dictionary<string, string> data, string token);
        (bool, string) EditCoursesGroup(Dictionary<string, string> data, string token);
        (bool, string) AddCourse(Dictionary<string, string> data, string token);
        (bool, string) EditCourse(Dictionary<string, string> data, string token);
        (bool, string) AddLesson(Dictionary<string, string> data, string token);
        (bool, string) EditLesson(Dictionary<string, string> data, string token);
    }
}
