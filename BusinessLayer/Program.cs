using System;
using BusinessLayer.Users;

namespace BusinessLayer
{
    using Controllers; // todo elo z tym potem
    using Facade;
    using System.Collections.Generic;

    class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SystemFactory().System;

            var (_, token) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "haslo1234",
            });

            //var (_, message) = new CourseGroupController().CheckPermissions(LoggedUsers.GetInstance().GetUser(token), 1);

            var test = facade.ShowField(new Dictionary<string, string>
            {
                ["id"] = "1",
            }, token);
        }
    }
}