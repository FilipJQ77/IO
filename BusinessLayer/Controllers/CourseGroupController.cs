using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Users;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Controllers
{
    class CourseGroupController
    {
        public (bool, string) EditCoursesGroup(Dictionary<string, string> data, string token)
        {
            if (new UserController().CheckRank(token) != Rank.Administrator)
                return (false, "Tylko administrator może edytować kurs");

            var cgRepo = new RepositoryFactory().GetRepository<CourseGroup>();
            var fieldRepo = new RepositoryFactory().GetRepository<Field>();

            if (!data.ContainsKey("id") || !int.TryParse(data["id"], out int id) || id < 1)
                return (false, "Nieprawidłowe id");

            if (!data.ContainsKey("ects") || !int.TryParse(data["ects"], out int ects) || ects < 1)
                return (false, "Nieprawidłowa liczba ects");

            if (!data.ContainsKey("semester") || !int.TryParse(data["semester"], out int semester) || semester < 1)
                return (false, "Nieprawidłowy numer semestru");

            if (!data.ContainsKey("fieldId") || !int.TryParse(data["fieldId"], out int fieldId) || fieldId < 1)
                return (false, "Nieprawidłowe id kierunku");

            if (!data.ContainsKey("code") || data["code"] == "")
                return (false, "Kod kursu nie może być pusty");

            if (!data.ContainsKey("name") || data["name"] == "")
                return (false, "Nazwa kursu nie może być pusta");

            var courseGroup = cgRepo.GetDetail(p => p.Id == id);
            if(courseGroup == null)
                return (false, "Nieprawidłowe id");

            var field = fieldRepo.GetDetail(p => p.Id == fieldId);
            if (field == null)
                return (false, "Nieprawidłowe id kierunku");

            courseGroup.Code = data["code"];
            courseGroup.NumberOfEcts = ects;
            courseGroup.Semester = semester;
            courseGroup.Field = field;
            courseGroup.Name = data["name"];

            cgRepo.SaveChanges();

            return (true, "Zmieniono grupę kursów");
        }

        public (bool, string) CheckPermissions(IUser user, int courseGroupId)
        {
            if (user == null)
                return (false, "Należy się zalogować");
            if (user.User.Rank == Rank.Administrator)
                return (true, "");

            var studentData = (user as Student).StudentData;
            var cgRep = new RepositoryFactory().GetRepository<CourseGroup>();
            var courseGroup = cgRep.GetDetail(p => p.Id == courseGroupId);
            if (courseGroup == null || courseGroup.FieldId != studentData.FieldId)
                return (false, "Nie należysz do kierunku wybranej grupy kursów");
            if (courseGroup.Semester != studentData.Semester)
                return (false, "Nie należysz do semestru, na którym grupa kursów się odbywa");

            if (studentData.RegistrationDate == null)
                return (false, "Nie przydzielono ci jeszcze terminu zapisów");

            if (DateTime.Now < studentData.RegistrationDate)
                return (false, $"Twój termin zapisów wypada dopiero {studentData.RegistrationDate.ToString()}");

            return (true, "");
        }
    }
}
