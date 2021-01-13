using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Users;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Controllers
{
    public class LessonController
    {
        public (bool, string) SignToLesson(Dictionary<string, string> data, string token)
        {
            var userRank = new UserController().CheckRank(token);

            var loggedUser = LoggedUsers
                .GetInstance()
                .GetUser(token);

            var repoFactory = new RepositoryFactory();
            var studentRepo = repoFactory.GetRepository<StudentData>();
            var lessonRepo = repoFactory.GetRepository<Lesson>();

            int studentId;
            StudentData student;

            switch (userRank)
            {
                case Rank.None:
                    return (false, "Należy się zalogować");
                case Rank.Administrator:
                    if (!data.ContainsKey("studentId") || !int.TryParse(data["studentId"], out studentId))
                        return (false, "Niepoprawne ID studenta");
                    student = studentRepo.GetDetail(s => s.Id == studentId);
                    break;
                default: // student
                    student = (loggedUser as Student).StudentData;
                    break;
            }

            if (!data.ContainsKey("lessonId") || !int.TryParse(data["lessonId"], out int lessonId))
                return (false, "Niepoprawne ID zajęć");

            var lesson = lessonRepo.GetDetail(l => l.Id == lessonId);
            if (lesson == null)
                return (false, "Nie ma lekcji o podanym ID");
            
            var course = lesson.Course;
            var courseGroup = lesson.Course.CourseGroup;
            var (canSign, err) = new CourseGroupController().CheckPermissions(loggedUser,courseGroup.Id);
            if (!canSign)
                return (false, err);

            if (student.Lessons.Any(l => l.Course == course))
                return (false, "Student jest już zapisany na kurs");

            if (lesson.Space <= 0 && userRank != Rank.Administrator)
                return (false, "Brak miejsc");

            student.Lessons.Add(lesson);
            lesson.Space--;

            studentRepo.SaveChanges();
            lessonRepo.SaveChanges();

            return (true, "Zapis na zajęcia udał się");
        }
    }
}