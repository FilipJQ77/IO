using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Facade;

namespace BusinessLayer.Tests.fitnesse
{
    public class AddAccountStudentTest : fit.ColumnFixture
    {
        public string Login;
        public string Rank;
        public string FirstName;
        public string LastName;
        public string Index;
        public string Semester;
        public string FieldId;
        public string Comment;

        public bool AddAccountStudent()
        {
            var facade = new SystemFactory().System;
            var (_, token) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "Pass1",
            });
            var (added, _) = facade.AddAccount(new Dictionary<string, string>
            {
                ["login"] = Login,
                ["rank"] = Rank,
                ["firstName"] = FirstName,
                ["lastName"] = LastName,
                ["index"] = Index,
                ["fieldId"] = FieldId,
                ["semester"] = Semester
            }, token);

            facade.LogOut(token);

            return added;
        }
    }
}