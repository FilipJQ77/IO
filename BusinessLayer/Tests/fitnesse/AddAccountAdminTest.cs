using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Facade;
using BusinessLayer.Users;
using DataAccess.BusinessObjects.Entities;

namespace BusinessLayer.Tests.fitnesse
{
    public class AddAccountAdminTest : fit.ColumnFixture
    {
        public string Login;
        public string Rank;
        public string Comment;

        public bool AddAccountAdmin()
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
                ["rank"] = Rank
            }, token);

            facade.LogOut(token);

            return added;
        }
    }
}