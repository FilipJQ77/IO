using System;

namespace BusinessLayer
{
    using Facade;
    using System.Collections.Generic;

    class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SystemFactory().System;

            var (_, tokenAdmin) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Admin",
                ["password"] = "haslo1234",
            });

            var (_, tokenStudent) = facade.LogIn(new Dictionary<string, string>
            {
                ["login"] = "Student1",
                ["password"] = "haslo1234",
            });

            facade.LogOut(tokenStudent);

            var (isAssigned, err) = facade.AssignRegistrationDate(new Dictionary<string, string>
            {
                ["dateStart"] = "2020-01-08 11:00",
                ["dateEnd"] = "2020-01-08 12:30",
            }, tokenAdmin);

            var (isAssigned2, err2) = facade.AssignRegistrationDate(new Dictionary<string, string>
            {
                ["dateStart"] = "2020-01-08 12:30",
                ["dateEnd"] = "2020-01-08 11:00",
            }, tokenAdmin);

            var (added, err3) = facade.AddAccount(new Dictionary<string, string>
            {

            }, tokenStudent);

            var (edited, err4) = facade.EditCoursesGroup(new Dictionary<string, string>
            {
                ["id"]="1",
                ["ects"]="29",
                ["semester"]="6",
                ["fieldId"]="2",
                ["code"]="INEK000420",
                ["name"]="Analiza matematyczna 4",
            }, tokenAdmin);

            var test = facade.ShowField(new Dictionary<string, string>
            {
                ["id"] = "1",
            }, tokenAdmin);
        }
    }
}