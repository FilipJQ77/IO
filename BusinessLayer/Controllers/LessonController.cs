using System.Collections.Generic;
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

            int studentId;
            Student student;

            switch (userRank)
            {
                case Rank.None:
                    return (false, "Należy się zalogować");
                case Rank.Administrator:
                    if (!data.ContainsKey("studentId") || !int.TryParse(data["studentId"], out studentId))
                        return (false, "Niepoprawne ID studenta");
                    break;
            }
            var repoFactory = new RepositoryFactory();
            var studentRepo = repoFactory.GetRepository<Student>();
            var student = studentRepo.GetDetail();

            var loggedUser = LoggedUsers
                .GetInstance()
                .GetUser(token);

            var xd = new CourseGroupController().CheckPermissions(loggedUser);
        }
    }
}