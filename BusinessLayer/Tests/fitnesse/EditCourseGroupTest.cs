using BusinessLayer.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tests.fitnesse
{
    public class EditCourseGroupTest : fit.ColumnFixture
    {
        public string Login;
        public string Password;

        public string Id;
        public string Ects;
        public string Semester;
        public string FieldId;
        public string Code;
        public string Name;

        public string token;

        public string LastMessage;
        public string Comment;

        public bool loginUser()
        {
            var system = new SystemFactory().System;

            var (passed, ms) = system.LogIn(new Dictionary<string, string>
            {
                ["login"] = Login,
                ["password"] = Password,
            });

            token = ms;

            return passed;
        }

        public bool EditCourseGroup()
        {
            var system = new SystemFactory().System;

            var (passed, err) = system.EditCoursesGroup(new Dictionary<string, string>
            {
                ["id"] = Id,
                ["ects"] = Ects,
                ["semester"] = Semester,
                ["fieldId"] = FieldId,
                ["code"] = Code,
                ["name"] = Name,
            }, token);

            LastMessage = err;

            return passed;
        }

        public void LogOut()
        {
            var system = new SystemFactory().System;
            system.LogOut(token);
        }
    }
}
